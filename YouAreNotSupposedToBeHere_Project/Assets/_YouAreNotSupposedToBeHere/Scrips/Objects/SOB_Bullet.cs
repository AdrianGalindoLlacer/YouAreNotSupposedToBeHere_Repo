using UnityEngine;

public class SOB_Bullet : MonoBehaviour
{
    //[SerializeField]Animator anim;
    [SerializeField] float speed = 3f;
    [SerializeField] float damage = 30;
    MAN_Health Health;
    MAN_SceneManager Manager;
    private bool Move = true;


    private void Start()
    {
        Health = find
    }


    private void Update()
    {
        if (!Move)
        {
            //anim.SetTrigger("Impact");
            return;

        }
        else
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall")|| collision.gameObject.CompareTag("Robot"))
        {
            Move = false;
            Destroy(gameObject, 0f); //se quitara cuando esten las animaciones
            //anim.Settrigger("Impact");
        }
        if (collision.gameObject.CompareTag("Guard"))
        {
            MAN_Health.health = MAN_Health.health - damage;
            if (MAN_Health.health <= 0)
            {
                MAN_SceneManager.Restart();
            }
        }
    }
}
