using System.Collections;
using UnityEngine;

public class ENE_Camera : MonoBehaviour
{
    Animator anim;
    [SerializeField] GameObject postit; //Guarda el objeto hijo el cual se le pone en la lente a la camara
    [SerializeField] GameObject Vision; //Variable que guarda su rango de "daño"
    [SerializeField] float stunTime = 8f; //Duracion del PostIt en la lente
    private Coroutine stunroutine;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("SBullet"))
        {
            anim.SetTrigger("Hit");
            if (stunroutine != null)
            {
                StopCoroutine(stunroutine);
            }
            stunroutine = StartCoroutine(StunRoutine());

        }
    }

    private IEnumerator StunRoutine()
    {
        Vision.SetActive(false);
        postit.SetActive(true);
        yield return new WaitForSeconds(stunTime); // Duración del PostIt en la lente
        Vision.SetActive(true);
        postit.SetActive(false);
    }
}
