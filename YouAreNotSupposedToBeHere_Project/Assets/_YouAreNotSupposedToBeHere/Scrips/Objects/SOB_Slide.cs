using UnityEngine;

public class SOB_Slide : MonoBehaviour
{

    private Animator anim;
    //[SerializeField] transform goal;


    private void Awake()
    {
        anim = GetComponent<Animator>();
    }



    private void Slide()
    {
        anim.SetTrigger("Shake");
    }
}
