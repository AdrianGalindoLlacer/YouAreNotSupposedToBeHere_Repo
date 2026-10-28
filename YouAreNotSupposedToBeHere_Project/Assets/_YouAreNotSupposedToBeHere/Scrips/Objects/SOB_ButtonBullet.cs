using UnityEngine;

public class SOB_ButtonBullet : MonoBehaviour
{
    private Animator anim;
    [SerializeField] Animator Door; // Animator de la puerta para mandarle señales
    private int count = 0; //Variable que guarda el estado en el que esta el boton (Cambiable por un bool)
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Bullet") || collision.gameObject.CompareTag("SBullet"))
        {
            Press();
        }
    }

    public void Press()
    {
        if (count >= 0)
        {
            anim.SetTrigger("Press");
            Door.SetBool("Open", true);
            count++;
        }
        else
        {
            anim.SetTrigger("Press");
            Door.SetBool("Open", false);
            count--;
        }
    }

    
}
