using UnityEngine;

public class SOB_Button : MonoBehaviour
{
    private Animator anim;
    [SerializeField] Animator Door;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Press()
    {
        anim.SetTrigger("Press");
        Door.SetTrigger("Open");
    }
}
