using Unity.VisualScripting;
using UnityEngine;

public class SOB_PresurePRob : MonoBehaviour
{
    Animator anim;
    private int count = 0;
    [SerializeField] Animator Door;
    [SerializeField] GameObject Robot;
    [SerializeField] GameObject RobotStand;
    [SerializeField] Transform Origin;
    [SerializeField] SOB_Robot RobotCont;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void Press()
    {
        if (count <= 0)
        {
            anim.SetBool("Press", false);
            Door.SetTrigger("Close");
        }
        else
        {
            anim.SetBool("Press", true);
            Door.SetTrigger("Open");
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
            RobotCont.Walk = false;
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
