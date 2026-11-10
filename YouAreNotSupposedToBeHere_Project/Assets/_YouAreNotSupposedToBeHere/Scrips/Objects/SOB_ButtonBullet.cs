using Unity.VisualScripting;
using UnityEngine;

public class SOB_ButtonBullet : MonoBehaviour
{
    [SerializeField] MAN_DoorSoundController soundC;
    private Animator anim;
    [SerializeField] Animator DoorRight;
    [SerializeField] Animator DoorLeft;
    private bool count = true; //Variable que guarda el estado en el que esta el boton (Cambiable por un bool)
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet") || collision.gameObject.CompareTag("SBullet"))
        {
            count = !count;
            Press();
        }
    }
    public void Press()
    {
        if (count)
        {
            Debug.Log("clos");
            soundC.playClose();
            DoorRight.SetBool("Open", false);
            DoorLeft.SetBool("Open", false);
            anim.SetTrigger("Press");
        }
        else
        {
            Debug.Log("openpli");
            soundC.playOpen();
            DoorRight.SetBool("Open", true);
            DoorLeft.SetBool("Open", true);
            anim.SetTrigger("Press");
        }
    }


}
