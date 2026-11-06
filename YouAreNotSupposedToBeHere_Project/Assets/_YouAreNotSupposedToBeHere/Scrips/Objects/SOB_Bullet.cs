using UnityEngine;

public class SOB_Bullet : MonoBehaviour
{
    [SerializeField] float speed = 3f;
    [SerializeField] MAN_Health Health;
    [SerializeField] MAN_SceneManager Manager;
    public bool Move = false;


    private void Update()
    {
        if (!Move)
        {
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
            gameObject.SetActive(false);
        }
        if (other.gameObject.CompareTag("Guard"))
        {
            Manager.gameover();
        }
    }
}
