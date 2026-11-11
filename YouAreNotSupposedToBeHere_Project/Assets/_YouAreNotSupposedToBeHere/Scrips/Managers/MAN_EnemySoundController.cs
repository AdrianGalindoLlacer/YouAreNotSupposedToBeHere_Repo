using UnityEngine;

public class MAN_EnemySoundController : MonoBehaviour
{
    [SerializeField] AudioSource audiosource;
    [SerializeField] AudioClip GameOver;

    public void playGO()
    {
        audiosource.PlayOneShot(GameOver);
    }
}
