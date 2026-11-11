using UnityEngine;

public class SOB_Robot : MonoBehaviour
{
    [SerializeField] Man_RobotSoundManager soundC;
    [SerializeField] float speed = 2f;
    public bool soundenabled;
    public bool Walk = false; //Variable que indica si el robot se mueve o no
    public bool sound = false; //Variable que activa el modo chase del guardia cuando es golpeado por una bala
    Rigidbody rb;
    Animator anim;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!soundenabled)
        {
            soundC.StopSound();
        }
        if (sound && soundenabled)
        {
            soundC.playSynergy();
        }
        if (!Walk)
        {
            if (soundenabled)
            {
                soundC.playNormal();
            }
            return;
        }
        else if (Walk)
        {
            if (soundenabled)
            {
                soundC.playWalk();
            }
            transform.Translate(speed * Time.deltaTime * Vector3.forward);
            anim.SetBool("Movement", true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall")  || collision.gameObject.CompareTag("Obstacle"))
        {
            anim.SetBool("Movement", false);
            anim.SetTrigger("Idle");
            Walk = false;
            sound = false;
            soundenabled = false;
        }
        if (collision.gameObject.CompareTag("Guard"))
        {
            anim.SetTrigger("Trip");
            Walk = false;
            soundenabled = false;
        }
        if (collision.gameObject.CompareTag("Bullet"))
        {
            sound = true;
            soundenabled = false;
        }
    }


}
