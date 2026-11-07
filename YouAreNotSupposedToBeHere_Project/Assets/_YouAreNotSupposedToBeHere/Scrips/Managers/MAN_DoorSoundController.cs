using UnityEngine;

public class MAN_DoorSoundController : MonoBehaviour
{
    [SerializeField] AudioSource audiosource;
    [SerializeField] AudioClip Open;
    //[SerializeField] AudioClip Close;

    public void playOpen()
    {
        audiosource.PlayOneShot(Open);
    }
    //public void playClose()
    //{
    //    audiosource.PlayOneShot(Close);
    //}
}
