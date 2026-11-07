using UnityEngine;

public class MAN_Player2SoundController : MonoBehaviour
{
    [SerializeField] AudioSource audiosource;
    [SerializeField] AudioClip Walk;
    [SerializeField] AudioClip Shoot;
    //[SerializeField] AudioClip Hide;
    //[SerializeField] AudioClip Slide;
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

    public void playShoot()
    {
        audiosource.PlayOneShot(Shoot);
    }

    //public void playHide()
    //{
    //    audiosource.PlayOneShot(Hide);
    //}
    //public void playSlide()
    //{
    //    audiosource.PlayOneShot(Slide);
    //}
}
