using UnityEngine;

// Ponlo en el personaje que quieres seguir viendo detras de los muros.
public class PSC_PlayerTroughWall : MonoBehaviour
{
    [Tooltip("Altura a la que se centra el agujero (aprox. el torso)")]
    [SerializeField] private float heightOffset = 1f;

    private static readonly int PlayerPosId = Shader.PropertyToID("_PlayerPos");

    private void LateUpdate()
    {
        Vector3 p = transform.position + Vector3.up * heightOffset;
        Shader.SetGlobalVector(PlayerPosId, new Vector4(p.x, p.y, p.z, 1f));
    }
}
