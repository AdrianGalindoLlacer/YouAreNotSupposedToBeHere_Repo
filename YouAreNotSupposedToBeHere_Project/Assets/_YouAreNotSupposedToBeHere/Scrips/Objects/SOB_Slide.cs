using UnityEngine;

public class SOB_Slide : MonoBehaviour
{

    private Animator anim;
    [SerializeField] Transform goal; // Variable que guarda el transform del empty en el que el jugador se teletransportara
    [SerializeField] GameObject Player; // Variable que guarda el player que se transportara


    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Transport()
    {
        if (Player.TryGetComponent(out Rigidbody rb))
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        Player.transform.position = goal.position;
        Physics.SyncTransforms();
    }

    public void Slide()
    {
        anim.SetTrigger("Shake");
    }
}
