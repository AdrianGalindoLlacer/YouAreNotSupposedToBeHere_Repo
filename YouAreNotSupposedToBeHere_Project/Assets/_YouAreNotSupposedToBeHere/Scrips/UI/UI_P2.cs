using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_P2 : MonoBehaviour
{
    //[SerializeField] private GameObject PlayerHide;
    private PSC_PlayerShoot2 PShoot;
    private PSC_PlayerController2 PControl;
    [SerializeField] private TMP_Text Bullets;
    [SerializeField] private Image BulletSprite;
    private GameObject HideSprite;

    private void Awake()
    {
        Bullets = GameObject.Find("Ammo_Text").GetComponent<TMP_Text>();
        PShoot = GetComponent<PSC_PlayerShoot2>();
        PControl = GetComponent<PSC_PlayerController2>();
        HideSprite = GameObject.Find("Hide");
    }

    private void Update()
    {
        if (PShoot.Ammo > 0)
        {
            Bullets.text = PShoot.Ammo.ToString();
            BulletSprite.color = Color.white;
        }
        else
        {
            Bullets.text = "";
            BulletSprite.color = Color.gray;
        }
        if (PControl.Hide)
        {
            HideSprite.SetActive(true);
        }
        else
        {
            HideSprite.SetActive(false);
        }
    }
}