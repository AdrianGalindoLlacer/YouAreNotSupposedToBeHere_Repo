using UnityEngine;

public class ENE_Guard : MonoBehaviour
{
    Animator anim;
    [SerializeField] float speed = 4f;
    //[SerializeField] float Rotate = 2f;
    [SerializeField] bool patrol = false; // Variable de la que depende que el guardia tenga o no movimiento de patruya
    [SerializeField] bool Hold = false;
    [SerializeField] bool Block = false;
    [SerializeField] GameObject RobotHold; // Variable que guarda el objeto del robot en su mano
    [SerializeField] GameObject RobotTarget; // Variable que guarda el robot al que busca cuando se active chase
    [SerializeField] GameObject RobotStand; // Variable que guarda el stand en el que dejara el muñeco
    [SerializeField] GameObject Vision; // Variable que guarda su rango de "daño"
    [SerializeField] GameObject ROrigin;
    [SerializeField] GameObject GOrigin;
    private bool patrolstate;
    private SOB_Robot Target;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        Target = RobotTarget.GetComponent<SOB_Robot>();
        patrolstate = patrol;
    }

    private void Update()
    {
        if (Block) return;
        if (Target.sound)
        {
            patrol = false;
            transform.position = Vector3.MoveTowards(transform.position, RobotTarget.transform.position, speed * Time.deltaTime);
        }
        else if (Hold)
        {
            transform.position = Vector3.MoveTowards(transform.position, RobotStand.transform.position, speed * Time.deltaTime);
        }
        else if (patrol)
        {
            //logica de patrulla
        }
        else
        {
            if (transform.position == GOrigin.transform.position)
            {
                patrol = patrolstate;
                return;
            }
            transform.position = Vector3.MoveTowards(transform.position, GOrigin.transform.position, speed * Time.deltaTime);
        }
    }
    private void Pick()
    {
        Target.sound = false;
        RobotHold.SetActive(true);
        RobotTarget.transform.position = ROrigin.transform.position;
        //anim.SetBool("Pick", false);
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
            patrol = false;
            Vision.SetActive(false);
            //anim.SetTrigger("Trip");
            //anim.SetBool("Pick", true);
        }
        if (collision.gameObject.CompareTag("Robot"))
        {
            Blocked();
            Vision.SetActive(false);
            //anim.SetBool("Pick", true);
            Block = true;
            Hold = true;
        }
        if (collision.gameObject.CompareTag("Stand") && Hold)
        {
            Blocked();
            //anim.SetTrigger("Lay");

        }
    }
}
