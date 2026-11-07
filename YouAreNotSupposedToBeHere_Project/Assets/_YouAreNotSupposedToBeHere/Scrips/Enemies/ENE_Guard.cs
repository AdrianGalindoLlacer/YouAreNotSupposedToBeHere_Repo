using UnityEngine;

public class ENE_Guard : MonoBehaviour
{
    [Header("Sound and animation Management")]
    [SerializeField] MAN_GuardSoundController soundC;
    private bool soundenabled;
    private bool animationSenable;
    Animator anim;

    [Header("Movement and behavior")]
    [SerializeField] float speed = 4f;
    //[SerializeField] float Rotate = 2f;
    [SerializeField] bool patrol = false; // Variable de la que depende que el guardia tenga o no movimiento de patruya
    [SerializeField] bool chase = false; // Variable de la que depende que el guardia este o no buscando al robot
    [SerializeField] bool Hold = false;
    [SerializeField] bool Block = false;
    private bool patrolstate;

    [Header("References")]
    [SerializeField] GameObject RobotHold; // Variable que guarda el objeto del robot en su mano
    [SerializeField] GameObject RobotTarget; // Variable que guarda el robot al que busca cuando se active chase
    [SerializeField] GameObject RobotStand; // Variable que guarda el stand en el que dejara el muñeco
    [SerializeField] GameObject StandRobot; //Variable que guarda el objeto del robot en el stand
    [SerializeField] GameObject Vision; // Variable que guarda su rango de "daño"
    [SerializeField] GameObject ROrigin;
    [SerializeField] GameObject GOrigin;
    private SOB_Robot Target;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        Target = RobotTarget.GetComponent<SOB_Robot>();
        patrolstate = patrol;
    }

    private void Update()
    {
        if (!soundenabled)
        {
            soundC.StopSound();
        }
        if (Block) return;
        if (Target.sound)
        {
            if (soundenabled && !animationSenable)
            {
                soundC.playWalk();
            }
            anim.SetBool("Walk", true);
            patrol = false;
            chase = true;
            transform.position = Vector3.MoveTowards(transform.position, RobotTarget.transform.position, speed * Time.deltaTime);
        }
        else if (Hold)
        {
            if (soundenabled && !animationSenable)
            {
                soundC.playWalk();
            }
            anim.SetBool("Walk", true);
            transform.position = Vector3.MoveTowards(transform.position, RobotStand.transform.position, speed * Time.deltaTime);
        }
        else if (patrol)
        {
            if (soundenabled && !animationSenable)
            {
                soundC.playWalk();
            }
            //logica de patrulla
            anim.SetBool("Walk", true);
        }
        else
        {
            if (transform.position == GOrigin.transform.position)
            {
                anim.SetBool("Walk", false);
                patrol = patrolstate;
                soundenabled = true;
                return;
            }
            transform.position = Vector3.MoveTowards(transform.position, GOrigin.transform.position, speed * Time.deltaTime);
            anim.SetBool("Walk", true);
        }
    }
    private void Pick()
    {
        //soundenabled = true;
        //soundC.playGrab();
        animationSenable = false;
        Target.sound = false;
        RobotHold.SetActive(true);
        RobotTarget.transform.position = ROrigin.transform.position;
    }

    private void Lay()
    {
        //soundenabled = true;
        //soundC.playLay();
        animationSenable = false;
        RobotHold.SetActive(false);
        StandRobot.SetActive(true);
        Hold = false;
    }

    void Blocked()
    {
        if (Block)
        {
            Block = false;
            return;
        }
        else
        {
            Block = true;
            return;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Robot") && patrol)
        {
            Blocked();
            anim.SetBool("Walk", false);
            soundenabled = false;
            animationSenable = true;
            patrol = false;
            Vision.SetActive(false);
            anim.SetTrigger("Trip");
        }
        else if (collision.gameObject.CompareTag("Stand") && Hold)
        {
            Blocked();
            anim.SetBool("Walk", false);
            soundenabled = false;
            animationSenable = true;
            anim.SetTrigger("Lay");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Robot") && chase)
        {
            anim.SetBool("Walk", false);
            soundenabled = false;
            animationSenable = true;
            Blocked();
            Vision.SetActive(false);
            anim.SetTrigger("Pick");
            Block = true;
            Hold = true;
            chase = false;
        }
    }
}
