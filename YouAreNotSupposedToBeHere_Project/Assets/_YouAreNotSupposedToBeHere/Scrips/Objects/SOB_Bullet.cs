using UnityEngine;

public class SOB_Bullet : MonoBehaviour
{
    //[SerializeField]Animator anim;
    [SerializeField] float speed = 3f;
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
            transform.Translate(speed * Time.deltaTime * Vector3.forward);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Robot"))
        {
            Move = false;
            Destroy(gameObject); //se quitara cuando esten las animaciones
            //anim.Settrigger("Impact");
        }
        if (other.gameObject.CompareTag("Guard"))
        {
                Manager.Restart();
        }
    }
}
