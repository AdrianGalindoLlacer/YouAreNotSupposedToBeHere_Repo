using UnityEngine;

public class ENE_Camera : MonoBehaviour
{
    Animator anim;
    [SerializeField] GameObject postit; //Guarda el objeto hijo el cual se le pone en la lente a la camara
    [SerializeField] GameObject Vision; //Variable que guarda su rango de "daño"
    //[SerializeField] float stunTime = 8f; //Duracion del PostIt en la lente


    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("SBullet"))
        {
            anim.SetTrigger("Hit");
            Vision.SetActive(false);
            postit.SetActive(true);
            // Hacer rutina
            Vision.SetActive(true);
            postit.SetActive(false);

        }
    }
}
