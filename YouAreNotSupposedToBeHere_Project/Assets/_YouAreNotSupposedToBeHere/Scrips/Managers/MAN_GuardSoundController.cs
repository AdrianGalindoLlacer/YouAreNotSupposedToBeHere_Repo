using UnityEngine;

public class MAN_GuardSoundController : MonoBehaviour
{
    [SerializeField] AudioSource audiosource;
    [SerializeField] AudioClip Walk;
    [SerializeField] AudioClip Lay;
    [SerializeField] AudioClip Grab;
    private void PlayLooped(AudioClip clip)
    {
        // Si ya está sonando este mismo clip, no hacemos nada (evita reinicios/cortes)
        if (audiosource.clip == clip && audiosource.isPlaying)
            return;

        audiosource.clip = clip;
        audiosource.loop = true;
        audiosource.Play();
    }
    public void StopSound()
    {
        audiosource.Stop();
        audiosource.clip = null;
    }

    public void playWalk()
    {
        PlayLooped(Walk);
    }

    public void playLay()
    {
        audiosource.PlayOneShot(Lay);
    }

    public void playGrab()
    {
        audiosource.PlayOneShot(Grab);
    }
}
