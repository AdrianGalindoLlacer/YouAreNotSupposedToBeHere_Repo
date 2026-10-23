using UnityEngine;

public class ENE_Guard : MonoBehaviour
{
    Animator anim;
    [SerializeField] float speed = 4f;
    [SerializeField] float Rotate = 2f;
    [SerializeField] bool patrol = false; // Variable de la que depende que el guardia tenga o no movimiento de patruya
    [SerializeField] bool chase = true; //Variable que indica que esta buscando el robot
    [SerializeField] GameObject RobotHold; // Variable que guarda el objeto del robot en su mano
    [SerializeField] GameObject RobotTarget; // Variable que guarda el robot al que busca cuando se active chase
    [SerializeField] GameObject RobotStand; // Variable que guarda el stand en el que dejara el muñeco
    [SerializeField] GameObject Vision; // Variable que guarda su rango de "daño"
    private SOB_Robot Target;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        Target = RobotTarget.GetComponent<SOB_Robot>();
    }

    private void Update()
    {
        if (Target.sound == true && chase == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, RobotTarget.transform.position, speed * Time.deltaTime);
            if (transform.position == RobotTarget.transform.position)
            {
                Vision.SetActive(false);
                anim.SetBool("Pick", true);
                chase = false;
            }
        }
    }
    private void pick()
    {
        Target.sound = false;
        RobotTarget.SetActive(false);
        anim.SetBool("Pick", false);

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Robot") && patrol)
        {
            patrol = false;
            Vision.SetActive(false);
            anim.SetTrigger("Trip");
            anim.SetBool("Pick", true);
        }
    }
}
