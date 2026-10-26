using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_P2 : MonoBehaviour
{
    //[SerializeField] private GameObject PlayerHide;
    [SerializeField] private PSC_PlayerController2 Controller;
    [SerializeField] private TMP_Text Bullets;
    [SerializeField] private TMP_Text PostIts;
    [SerializeField] private Image BulletSprite;
    [SerializeField] private Image SBulletSprite;



    private void Update()
    {
        if (Controller.Ammo > 0)
        {
            Bullets.text = Controller.Ammo.ToString();
            BulletSprite.color = Color.white;
        }
        else
        {
            Bullets.text = "";
            BulletSprite.color = Color.gray;
        }
        if (Controller.SpecialAmmo > 0)
        {
            PostIts.text = Controller.SpecialAmmo.ToString();
            SBulletSprite.color = Color.white;
        }
        else
        {
            PostIts.text = "";
            SBulletSprite.color = Color.gray;
        }
    }
}