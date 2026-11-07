using UnityEngine;

public class MAN_BulletSoundController : MonoBehaviour
{
    [SerializeField] AudioSource audiosource;
    [SerializeField] AudioClip Impact;

    public void playImpact()
    {
        audiosource.PlayOneShot(Impact);
    }
}
