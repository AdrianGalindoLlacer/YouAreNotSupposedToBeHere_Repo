using UnityEngine;

public class SOB_Button : MonoBehaviour
{
    private Animator anim;
    [SerializeField] Animator DoorRight;
    [SerializeField] Animator DoorLeft;
    private int count = 0; //Variable que guarda el estado en el que esta el boton (Cambiable por un bool)

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Press()
    {
        if (count == 0)
        {
            anim.SetTrigger("Press");
            DoorRight.SetBool("Open", false);
            DoorLeft.SetBool("Open", false);
            count++;
        }
        else
        {
            anim.SetTrigger("Press");
            DoorRight.SetBool("Open", true);
            DoorLeft.SetBool("Open", true);
            count--;
        }
    }
}
