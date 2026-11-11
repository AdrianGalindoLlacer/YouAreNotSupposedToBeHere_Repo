using UnityEngine;

public class SOB_Bullet : MonoBehaviour
{
    [SerializeField] MAN_BulletSoundController soundC;
    [SerializeField] float speed = 3f;
    [SerializeField] MAN_Health Health;
    [SerializeField] MAN_SceneManager Manager;
    public bool Move = false;

    private void Update()
    {
            transform.Translate(speed * Time.deltaTime * Vector3.forward);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Robot") || other.gameObject.CompareTag("Obstacle") || other.gameObject.CompareTag("Buttons"))
        {
                soundC.playImpact();
            gameObject.SetActive(false);
        }
        if (other.gameObject.CompareTag("Guard"))
        {
            Health.Health = 0;
            soundC.playImpact();
            Manager.gameover();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            gameObject.SetActive(false);
                soundC.playImpact();
        }

    }
}
