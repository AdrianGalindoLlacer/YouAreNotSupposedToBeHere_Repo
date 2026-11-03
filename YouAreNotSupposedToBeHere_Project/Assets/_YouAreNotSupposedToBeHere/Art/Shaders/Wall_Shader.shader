Shader "Custom/WallXRayHole"
{
    Properties
    {
        _BaseMap("Textura", 2D) = "white" {}
        _BaseColor("Color", Color) = (1,1,1,1)

        [Header(Agujero)]
        _HoleRadius("Radio del agujero (pantalla)", Range(0, 1)) = 0.15
        _HoleSoftness("Suavizado del borde", Range(0.001, 0.5)) = 0.08
        _MinAlpha("Alpha dentro del agujero", Range(0, 1)) = 0.0
        _OccluderMargin("Margen de oclusion (m)", Range(0, 2)) = 0.1

        [Header(Halo)]
        [HDR] _RingColor("Color del halo", Color) = (0.3, 0.8, 1, 0.6)
        _RingWidth("Grosor del halo", Range(0, 0.2)) = 0.03
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" "RenderPipeline"="UniversalPipeline" }

        Pass
        {
            Name "Forward"
            Tags { "LightMode"="UniversalForward" }

            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite On
            Cull Back

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            // Posicion del personaje, la rellena el script PlayerXRayTarget
            float4 _PlayerPos;

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                half4 _BaseColor;
                half _HoleRadius;
                half _HoleSoftness;
                half _MinAlpha;
                half _OccluderMargin;
                half4 _RingColor;
                half _RingWidth;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
                float2 uv         : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv         : TEXCOORD0;
                float3 positionWS : TEXCOORD1;
                float3 normalWS   : TEXCOORD2;
                float4 screenPos  : TEXCOORD3;
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                VertexPositionInputs p = GetVertexPositionInputs(IN.positionOS.xyz);
                OUT.positionCS = p.positionCS;
                OUT.positionWS = p.positionWS;
                OUT.screenPos = p.positionNDC;
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                return OUT;
            }

            // UV de pantalla (0-1) del personaje, mismo calculo que positionNDC
            float2 PlayerScreenUV()
            {
                float4 pc = TransformWorldToHClip(_PlayerPos.xyz);
                float2 ndc = float2(pc.x, pc.y * _ProjectionParams.x) * 0.5 + pc.w * 0.5;
                return ndc / pc.w;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                half4 tex = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv) * _BaseColor;

                // Iluminacion simple
                float3 n = normalize(IN.normalWS);
                Light l = GetMainLight();
                half3 light = l.color * saturate(dot(n, l.direction)) + SampleSH(n);
                half3 col = tex.rgb * light;

                // Distancia en pantalla al personaje (corregida por aspecto)
                float2 uv = IN.screenPos.xy / IN.screenPos.w;
                float2 d = uv - PlayerScreenUV();
                d.x *= _ScreenParams.x / _ScreenParams.y;
                float dist = length(d);

                // Solo si el muro esta entre la camara y el personaje
                float wallDist = distance(IN.positionWS, _WorldSpaceCameraPos);
                float playerDist = distance(_PlayerPos.xyz, _WorldSpaceCameraPos);
                float occluding = step(wallDist, playerDist - _OccluderMargin);

                // 1 dentro del agujero, 0 fuera
                float inner = _HoleRadius - _HoleSoftness;
                float hole = (1.0 - smoothstep(inner, _HoleRadius, dist)) * occluding;

                // Halo justo en el borde interior del agujero
                float ring = smoothstep(inner - _RingWidth, inner, dist) * hole;
                col = lerp(col, _RingColor.rgb, ring * _RingColor.a);

                half alpha = lerp(1.0, _MinAlpha, hole);
                alpha = max(alpha, ring * _RingColor.a);

                return half4(col, alpha * tex.a);
            }
            ENDHLSL
        }

        // Para que el muro siga proyectando sombra
        UsePass "Universal Render Pipeline/Lit/ShadowCaster"
    }
}
