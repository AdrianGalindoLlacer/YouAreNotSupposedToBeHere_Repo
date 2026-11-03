using System.Collections.Generic;
using UnityEngine;

// Ponlo en CADA personaje que quieras seguir viendo detras de los muros (maximo 8).
public class PSC_PlayerTroughWall : MonoBehaviour
{
    private const int MaxTargets = 8; // debe coincidir con MAX_PLAYERS del shader

    [Tooltip("Altura a la que se centra el agujero (aprox. el torso)")]
    [SerializeField] private float heightOffset = 1f;

    private static readonly List<PSC_PlayerTroughWall> Targets = new List<PSC_PlayerTroughWall>();
    private static readonly Vector4[] Positions = new Vector4[MaxTargets];
    private static readonly int PositionsId = Shader.PropertyToID("_PlayerPositions");
    private static readonly int CountId = Shader.PropertyToID("_PlayerCount");

    private void OnEnable() => Targets.Add(this);
    private void OnDisable() => Targets.Remove(this);

    private void LateUpdate()
    {
        // Solo el primero de la lista sube los datos al shader, una vez por frame
        if (Targets.Count == 0 || Targets[0] != this) return;

        int count = Mathf.Min(Targets.Count, MaxTargets);
        for (int i = 0; i < count; i++)
        {
            Vector3 p = Targets[i].transform.position + Vector3.up * Targets[i].heightOffset;
            Positions[i] = new Vector4(p.x, p.y, p.z, 1f);
        }

        Shader.SetGlobalVectorArray(PositionsId, Positions);
        Shader.SetGlobalFloat(CountId, count);
    }
}
