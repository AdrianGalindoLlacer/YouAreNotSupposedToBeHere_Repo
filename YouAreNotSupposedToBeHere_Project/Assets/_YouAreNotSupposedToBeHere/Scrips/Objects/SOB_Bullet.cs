using UnityEngine;

public class SOB_Bullet : MonoBehaviour
{
    //[SerializeField]Animator anim;
    [SerializeField] float speed = 3f;
    [SerializeField] float damage = 0.30f;
    [SerializeField] MAN_Health Health;
    [SerializeField] MAN_SceneManager Manager;
    public bool Move = false;


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
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Robot"))
        {
            Move = false;
            Destroy(gameObject, 0f); //se quitara cuando esten las animaciones
            //anim.Settrigger("Impact");
        }
        if (collision.gameObject.CompareTag("Guard"))
        {
            Health.Health = Health.Health - damage;
            if (Health.Health <= 0)
            {
                Manager.Restart();
            }
        }
    }
}
