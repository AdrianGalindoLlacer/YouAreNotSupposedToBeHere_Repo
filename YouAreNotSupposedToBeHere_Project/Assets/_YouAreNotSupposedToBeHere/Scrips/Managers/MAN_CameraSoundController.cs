using UnityEngine;
using UnityEngine.Audio;

public class MAN_CameraSoundController : MonoBehaviour
{
    AudioSource audiosource;
    [SerializeField] AudioClip Idle;

    private void Awake()
    {
        if (GameObject.Find("SK_SecurityCamera") == null)
        audiosource = GameObject.Find("SK_SecurityCamera").GetComponent<AudioSource>();
    }

    private void PlayLooped(AudioClip clip)
    {
        // Si ya está sonando este mismo clip, no hacemos nada (evita reinicios/cortes)
        if (audiosource.clip == clip && audiosource.isPlaying)
            return;

        audiosource.clip = clip;
        audiosource.loop = true;
        audiosource.Play();
    }

    public void playIdle()
    {
        PlayLooped(Idle);
    }
}
