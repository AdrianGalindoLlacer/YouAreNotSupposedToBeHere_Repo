using UnityEngine;

public class SOB_Robot : MonoBehaviour
{

    //[SerializeField]Animator anim;
    [SerializeField] float speed = 2f;
    public bool Walk = false;
    public bool sound = false;
    Rigidbody rb;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (!Walk)
        {
            //anim.SetBool("Movement", false);
            return;
        }
        else
        {
            Vector3 nextPosition = rb.position + transform.forward * speed * Time.fixedDeltaTime;
            rb.MovePosition(nextPosition);
            //anim.SetBool("Movement", true);
        }

        if (sound)
        {
            //Se incluye un sonido
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            sound = true;
        }
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Guard"))
        {
            Walk = false;
            sound = false;
        }
    }


}
