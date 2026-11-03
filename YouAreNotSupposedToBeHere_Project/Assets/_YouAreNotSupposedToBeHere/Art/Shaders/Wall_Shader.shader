Shader "Custom/WallXRayHole"
{
    Properties
    {
        _BaseMap("Textura", 2D) = "white" {}
        _BaseColor("Color", Color) = (1,1,1,1)
        _Smoothness("Smoothness", Range(0, 1)) = 0.5
        _Metallic("Metallic", Range(0, 1)) = 0.0

        [Header(Agujero)]
        _HoleRadius("Radio del agujero (pantalla)", Range(0, 1)) = 0.15
        _HoleSoftness("Suavizado del borde", Range(0.001, 0.5)) = 0.08
        _MinAlpha("Alpha dentro del agujero", Range(0, 1)) = 0.0
        _OccluderMargin("Margen de oclusion (m)", Range(0, 2)) = 0.1
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

            // Luces y sombras, igual que el shader Lit de URP
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
            #pragma multi_compile _ _ADDITIONAL_LIGHTS
            #pragma multi_compile _ _FORWARD_PLUS
            #pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile_fragment _ _SHADOWS_SOFT

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            // Posiciones de los personajes, las rellena el script PlayerXRayTarget
            #define MAX_PLAYERS 8
            float4 _PlayerPositions[MAX_PLAYERS];
            float _PlayerCount;

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                half4 _BaseColor;
                half _Smoothness;
                half _Metallic;
                half _HoleRadius;
                half _HoleSoftness;
                half _MinAlpha;
                half _OccluderMargin;
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
                float4 shadowCoord : TEXCOORD4;
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                VertexPositionInputs p = GetVertexPositionInputs(IN.positionOS.xyz);
                OUT.positionCS = p.positionCS;
                OUT.positionWS = p.positionWS;
                OUT.screenPos = p.positionNDC;
                OUT.shadowCoord = GetShadowCoord(p);
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                return OUT;
            }

            // UV de pantalla (0-1) de una posicion del mundo, mismo calculo que positionNDC
            float2 WorldToScreenUV(float3 worldPos)
            {
                float4 pc = TransformWorldToHClip(worldPos);
                float2 ndc = float2(pc.x, pc.y * _ProjectionParams.x) * 0.5 + pc.w * 0.5;
                return ndc / pc.w;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                half4 tex = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv) * _BaseColor;

                // ---- Agujero ----
                float2 uv = IN.screenPos.xy / IN.screenPos.w;
                float aspect = _ScreenParams.x / _ScreenParams.y;
                float wallDist = distance(IN.positionWS, _WorldSpaceCameraPos);
                float inner = _HoleRadius - _HoleSoftness;

                float hole = 0.0;
                for (int i = 0; i < MAX_PLAYERS; i++)
                {
                    if (i >= (int)_PlayerCount) break;

                    float3 playerPos = _PlayerPositions[i].xyz;

                    float2 d = uv - WorldToScreenUV(playerPos);
                    d.x *= aspect;
                    float dist = length(d);

                    float playerDist = distance(playerPos, _WorldSpaceCameraPos);
                    float occluding = step(wallDist, playerDist - _OccluderMargin);

                    float h = (1.0 - smoothstep(inner, _HoleRadius, dist)) * occluding;
                    hole = max(hole, h);
                }

                half alpha = lerp(1.0, _MinAlpha, hole);

                // ---- Iluminacion PBR (igual que el shader Lit) ----
                InputData inputData = (InputData)0;
                inputData.positionWS = IN.positionWS;
                inputData.normalWS = normalize(IN.normalWS);
                inputData.viewDirectionWS = GetWorldSpaceNormalizeViewDir(IN.positionWS);
                inputData.shadowCoord = IN.shadowCoord;
                inputData.bakedGI = SampleSH(inputData.normalWS);
                inputData.shadowMask = half4(1, 1, 1, 1);
                inputData.normalizedScreenSpaceUV = GetNormalizedScreenSpaceUV(IN.positionCS);

                SurfaceData surfaceData = (SurfaceData)0;
                surfaceData.albedo = tex.rgb;
                surfaceData.metallic = _Metallic;
                surfaceData.smoothness = _Smoothness;
                surfaceData.occlusion = 1.0;
                surfaceData.alpha = alpha * tex.a;

                return UniversalFragmentPBR(inputData, surfaceData);
            }
            ENDHLSL
        }

        // Para que el muro siga proyectando sombra
        UsePass "Universal Render Pipeline/Lit/ShadowCaster"
    }
}
