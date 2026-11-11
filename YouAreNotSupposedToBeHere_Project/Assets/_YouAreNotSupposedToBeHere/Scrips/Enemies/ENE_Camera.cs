using System.Collections;
using UnityEngine;

public class ENE_Camera : MonoBehaviour
{
    [SerializeField] MAN_CameraSoundController soundC;
    Animator anim;
    [SerializeField] GameObject Vision; //Variable que guarda su rango de "daño"
    [SerializeField] float stunTime = 8f; //Duracion del PostIt en la lente
    private Coroutine stunroutine;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        soundC.playIdle();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            anim.SetTrigger("Hit");
            stunroutine = StartCoroutine(StunRoutine());

        }
    }

    private IEnumerator StunRoutine()
    {
        Vision.SetActive(false);
        yield return new WaitForSeconds(stunTime); // Duración del PostIt en la lente
        Vision.SetActive(true);
    }
}
