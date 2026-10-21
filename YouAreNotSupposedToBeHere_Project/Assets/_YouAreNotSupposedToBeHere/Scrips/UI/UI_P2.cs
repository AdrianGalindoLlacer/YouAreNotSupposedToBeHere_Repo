using TMPro;
using UnityEngine;

public class UI_P2 : MonoBehaviour
{
    [SerializeField] private GameObject PlayerHide;
    [SerializeField] private PSC_PlayerController2 Controller;
    [SerializeField] private TMP_Text Bullets;
    [SerializeField] private TMP_Text PostIts;
    [SerializeField] private - BulletFrame;
    [SerializeField] private - SBulletFrame;



    private void Update()
    {
        if (Controller.Ammo > 0)
        Bullets.text = Controller.Ammo.ToString();
        else
        {
            BulletFrame.color = Color.darkViolet;
        }
        if (Controller.SpecialAmmo > 0)
            Bullets.text = Controller.SpecialAmmo.ToString();
        else
        {
            SBulletFrame.color = Color.darkViolet;
        }
    }
}
