using UnityEngine;
using UnityEngine.Audio;

public class MAN_SoundController : MonoBehaviour
{
    [SerializeField] AudioSource audiosource;
    [SerializeField] AudioClip GameOver;

    public void playGO()
    {
        audiosource.PlayOneShot(GameOver);
    }
}
