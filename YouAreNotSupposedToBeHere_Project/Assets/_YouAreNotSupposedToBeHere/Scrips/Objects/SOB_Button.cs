using UnityEngine;

public class SOB_Button : MonoBehaviour
{
    private Animator anim;
    //[SerializeField] Animator Door; // Animator de la puerta para mandarle señales
    private int count = 0; //Variable que guarda el estado en el que esta el boton (Cambiable por un bool)
    [SerializeField] GameObject DoorRight;
    [SerializeField] GameObject DoorLeft;

    //void Awake()
    //{
    //    anim = GetComponent<Animator>();
    //}

    public void Press()
    {
        if (count == 0)
        {
            DoorLeft.SetActive(false);
            DoorRight.SetActive(false);
            //anim.SetTrigger("Press");
            //Door.SetBool("Open", true);
            count++;
        }
        else
        {
            DoorLeft.SetActive(true);
            DoorRight.SetActive(true);
            //anim.SetTrigger("Press");
            //Door.SetBool("Open", false);
            count--;
        }
    }
}
