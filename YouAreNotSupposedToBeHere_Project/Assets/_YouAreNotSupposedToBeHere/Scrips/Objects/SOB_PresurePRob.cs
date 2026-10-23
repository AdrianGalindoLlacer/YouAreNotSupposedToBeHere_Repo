using UnityEngine;

public class SOB_PresurePRob : MonoBehaviour
{
    Animator anim;
    private int count = 0;
    [SerializeField] Animator Door;

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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Box") || collision.gameObject.CompareTag("Robot"))
        {
            count++;
            Press();
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Box") || collision.gameObject.CompareTag("Robot"))
        {
            count--;
            Press();
        }
    }
}
