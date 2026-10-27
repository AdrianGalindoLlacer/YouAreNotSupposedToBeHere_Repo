using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_P1 : MonoBehaviour
{
    [SerializeField] private PSC_PlayerController1 Controller;
    [SerializeField] private Image RobotSprite;



    private void Update()
    {
        if (!Controller.HoldRobot)
            RobotSprite.color = Color.gray;
        else
        {
            RobotSprite.color = Color.white;
        }
    }
}