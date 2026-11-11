using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class MAN_Health : MonoBehaviour
{
    public float Health = 1f;
    [SerializeField] Image image;
    [SerializeField] Sprite[] GOframes;
    [SerializeField] float framerate;
    [SerializeField] Image GO;
    public bool Dcount = false;
    private int currentFrame;
    private float timer;

    private void Update()
    {
        image.fillAmount = Health;
        if (Health <= 0)
        {
            timer += Time.deltaTime;
            float frameDuration = 1f / framerate;

            if (timer >= frameDuration)
            {
                timer -= frameDuration;
                currentFrame++;

                if (currentFrame >= GOframes.Length) currentFrame = 0;
            }
            GO.sprite = GOframes[currentFrame];
        }
    }

}
