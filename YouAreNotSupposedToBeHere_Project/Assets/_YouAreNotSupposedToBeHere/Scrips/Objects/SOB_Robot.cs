using UnityEngine;

public class SOB_Robot : MonoBehaviour
{

    //[SerializeField]Animator anim;
    [SerializeField] float speed = 2f;
    public bool Walk = false; //Variable que indica si el robot se mueve o no
    public bool sound = false; //Variable que activa el modo chase del guardia cuando es golpeado por una bala
    Rigidbody rb;
    Animator anim;


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
            //anim.SetTrigger("Sinergy");
            sound = true;
        }
        if (collision.gameObject.CompareTag("Wall"))
        {
            //anim.SetTrigger("Idle");
            Walk = false;
            sound = false;
        } if (collision.gameObject.CompareTag("Guard"))
        {
            //anim.SetTrigger("Trip");
            Walk = false;
            sound = false;
        }
    }


}
