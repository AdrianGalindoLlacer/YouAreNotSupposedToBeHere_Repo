using UnityEngine;
using UnityEngine.UI;

public class UI_PopUps : MonoBehaviour
{
    [Header("Activation")]
    [SerializeField] GameObject popUp;
    private int count;

    [Header("Function")]
    [SerializeField] Sprite[] popUpframes;
    [SerializeField] float framerate;
    [SerializeField] Image image;
    private int currentFrame;
    private float timer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            count++;
            if (count > 0)
            {
                popUp.SetActive(true);
                currentFrame = 0;
                timer = 0f;

                image.sprite = popUpframes[0];
            }
        }
    }



    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            count--;
            if (count == 0)
            {
                popUp.SetActive(false);
            }
        }
    }

    private void Update()
    {

        timer += Time.deltaTime;
        float frameDuration = 1f / framerate;

        if (timer >= frameDuration)
        {
            timer -= frameDuration;
            currentFrame++;

            if (currentFrame >= popUpframes.Length) currentFrame = 0;
        }
        image.sprite = popUpframes[currentFrame];
    }
}