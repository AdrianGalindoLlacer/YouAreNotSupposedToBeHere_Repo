using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_P1 : MonoBehaviour
{
    [SerializeField] private PSC_PlayerController1 Controller;
    [SerializeField] Image Frame;
    [SerializeField] Sprite RobotSprite;
    [SerializeField] Sprite[] RobotWalk;
    [SerializeField] float framerate = 1;
    private float timer;
    private int currentFrame;


    private void Update()
    {
        if (!Controller.HoldRobot)
            Frame.color = Color.gray;
        else
        {
            Frame.color = Color.white;
        }
        if (Controller.WalkRobot)
        {
            timer += Time.deltaTime;
            float frameDuration = 1f / framerate;

            if (timer >= frameDuration)
            {
                timer -= frameDuration;
                currentFrame++;

                if (currentFrame >= RobotWalk.Length) currentFrame = 0;
            }
            Frame.sprite = RobotWalk[currentFrame];
        }
        else
        {
            Frame.sprite = RobotSprite;
        }

    }
}