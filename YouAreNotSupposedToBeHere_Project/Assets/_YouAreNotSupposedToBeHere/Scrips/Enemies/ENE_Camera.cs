using UnityEngine;

public class ENE_Camera : MonoBehaviour
{
    Animator anim;
    [SerializeField] GameObject postit;
    [SerializeField] GameObject Vision;
    [SerializeField] float stunTime = 8f;


    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("SBullet"))
        {
            anim.SetTrigger("Hit");
            Vision.SetActive(false);
            postit.SetActive(true);
            // Hacer rutina
            Vision.SetActive(true);

        }
    }
}
