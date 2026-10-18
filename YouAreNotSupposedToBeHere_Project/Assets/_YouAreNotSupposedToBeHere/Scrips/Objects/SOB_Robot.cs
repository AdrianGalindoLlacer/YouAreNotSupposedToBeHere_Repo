using UnityEngine;

public class SOB_Robot : MonoBehaviour
{

    Animator anim;
    [SerializeField] float speed = 2f;
    public bool Walk = false;
    private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (!Walk)
        {
            anim.SetBool("Movement", false);
            return;
        }
        else
        {
            Vector3 nextPosition = rb.position + transform.forward * speed * Time.fixedDeltaTime;
            rb.MovePosition(nextPosition);
            anim.SetBool("Movement", true);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Sound();
        }
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Guard"))
        {
            Walk = false;
        }
    }

    private void Sound()
    {

    }


}
