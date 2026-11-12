using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UI_PopUps : MonoBehaviour
{
    [Header("Activation")]
    [SerializeField] GameObject popUp;
    private int count;

    [Header("Function")]
    [SerializeField] Sprite[] popUpframes;
    [SerializeField] float framerate;
    [SerializeField] Image image;
    [SerializeField] float duration = 5f;
    [SerializeField] GameObject player; 
    private int currentFrame;
    private float timer;

    private Coroutine PopUpRoutine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
                popUp.SetActive(true);
                currentFrame = 0;
                timer = 0f;
                image.sprite = popUpframes[0];
                if (duration > 0)
                {
                    PopUpRoutine = StartCoroutine(PopUpDurationRoutine());
                }
        }
    }

    private IEnumerator PopUpDurationRoutine()
    {
        yield return new WaitForSeconds(duration);
        popUp.SetActive(false);
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            popUp.SetActive(false);
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