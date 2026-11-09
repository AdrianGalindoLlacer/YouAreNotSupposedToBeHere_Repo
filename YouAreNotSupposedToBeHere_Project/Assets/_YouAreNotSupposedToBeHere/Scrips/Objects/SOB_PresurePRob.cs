using Unity.VisualScripting;
using UnityEngine;

public class SOB_PresurePRob : MonoBehaviour
{
    [SerializeField] MAN_DoorSoundController soundC;
    Animator anim;
    [SerializeField]private int count = 0;
    [SerializeField] Animator DoorRight;
    [SerializeField] Animator DoorLeft;
    [SerializeField] GameObject Robot; //Robot que se lanza y se deja caer por el mapa
    [SerializeField] GameObject RobotStand; //Visual del robot que esta siempre encima pero generalmente desactivado
    [SerializeField] Transform Origin; //origen del robot al cual se le manda cuando entra en contacto
    [SerializeField] SOB_Robot RobotCont; //Script del robot que se le manda el bool Walk = false para que deje de caminar y se quede quieto

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void Press()
    {
        if (count == 0)
        {
            soundC.playClose();
            anim.SetBool("Press", false);
            DoorRight.SetBool("Open", false);
            DoorLeft.SetBool("Open", false);
        }
        else
        {
            soundC.playOpen();
            anim.SetBool("Press", true);
            DoorRight.SetBool("Open", true);
            DoorLeft.SetBool("Open", true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Box"))
        {
            count++;
            Press();
        }
        if (other.gameObject.CompareTag("Robot"))
        {
            count++;
            Press();
            RobotCont.Walk = false;
            RobotCont.soundenabled = false;
            Robot.transform.position = Origin.transform.position;
            RobotStand.SetActive(true);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Box"))
        {
            count--;
            Press();
        }
    }
}
