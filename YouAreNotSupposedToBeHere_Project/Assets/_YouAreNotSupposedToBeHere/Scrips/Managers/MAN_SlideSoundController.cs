using UnityEngine;

public class MAN_SlideSoundController : MonoBehaviour
{
    [SerializeField] AudioSource audiosource;
    [SerializeField] AudioClip Enter;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void playEnter()
    {
        audiosource.PlayOneShot(Enter);
    }
}
