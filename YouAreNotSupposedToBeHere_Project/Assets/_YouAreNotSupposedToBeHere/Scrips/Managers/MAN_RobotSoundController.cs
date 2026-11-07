using UnityEngine;

public class Man_RobotSoundManager: MonoBehaviour
{

    public AudioSource audiosource;
    [SerializeField] AudioClip RobotSound;
    [SerializeField] AudioClip RobotWalk;
    [SerializeField] AudioClip Synergy;


    private void PlayLooped(AudioClip clip)
    {
        // Si ya está sonando este mismo clip, no hacemos nada (evita reinicios/cortes)
        if (audiosource.clip == clip && audiosource.isPlaying)
            return;

        audiosource.clip = clip;
        audiosource.loop = true;
        audiosource.Play();
    }

    public void playNormal()
    {
        PlayLooped(RobotSound);
    }

    public void playWalk()
    {
        PlayLooped(RobotWalk);
    }

    public void playSynergy()
    {
        PlayLooped(Synergy);
    }

    public void StopSound()
    {
        audiosource.Stop();
        audiosource.clip = null;
    }
}
