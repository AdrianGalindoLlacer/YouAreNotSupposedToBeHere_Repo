using UnityEngine;

public class SOB_PresureP : MonoBehaviour
{
    Animator anim;
    private int count = 0;
    [SerializeField] Animator Door;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    // Update is called once per frame

    private void Update()
    {
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
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Box"))
        {
            count++;
            Press();
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Box"))
        {
            count--;
            Press();
        }
    }
}
