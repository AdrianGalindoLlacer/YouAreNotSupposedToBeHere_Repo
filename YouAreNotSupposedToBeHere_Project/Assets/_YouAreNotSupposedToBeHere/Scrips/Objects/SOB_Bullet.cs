using UnityEngine;

public class SOB_Bullet : MonoBehaviour
{
    //[SerializeField] MAN_BulletSoundController soundC;
    [SerializeField] float speed = 3f;
    [SerializeField] MAN_Health Health;
    [SerializeField] MAN_SceneManager Manager;
    public bool Move = false;
    private bool soundenabled = true;

    private void Update()
    {
        if (!Move)
        {
            transform.position = Vector3.zero;
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
            gameObject.SetActive(false);
            if (soundenabled)
            {
                //soundC.playImpact();
                soundenabled = false;
            }
        }
        if (other.gameObject.CompareTag("Guard"))
        {
            Manager.gameover();
        }
    }
}
