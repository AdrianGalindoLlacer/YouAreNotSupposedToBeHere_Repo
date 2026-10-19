using UnityEngine;

public class SOB_ButtonBullet : MonoBehaviour
{
    private Animator anim;
    [SerializeField] Animator Door;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet") || collision.gameObject.CompareTag("SBullet"))
        {
            Press();
        }
    }

    public void Press()
    {
        anim.SetTrigger("Press");
        Door.SetTrigger("Open");
    }

    
}
