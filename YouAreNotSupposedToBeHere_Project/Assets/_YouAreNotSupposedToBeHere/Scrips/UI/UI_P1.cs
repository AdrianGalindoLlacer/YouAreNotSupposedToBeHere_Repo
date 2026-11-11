using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_P1 : MonoBehaviour
{
    [SerializeField] private PSC_PlayerController1 Controller;
    [SerializeField] Image Frame;
    [SerializeField] Sprite RobotSprite;
    [SerializeField] Sprite RobotWalk;



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
            Frame.sprite = RobotWalk;
        }
        else
        {
            Frame.sprite = RobotSprite;
        }

    }
}