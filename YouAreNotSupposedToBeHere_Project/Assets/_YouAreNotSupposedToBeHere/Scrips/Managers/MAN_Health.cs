using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class MAN_Health : MonoBehaviour
{
    [Header("GameOver")]
    [SerializeField] Sprite[] GOframes;
    [SerializeField] float framerate;
    public bool Dcount = false;
    public float Health = 1f;
    private MAN_SceneManager manager;
    private Image image;
    private Image GO;
    private int currentFrame;
    private float timer;

    [Header("Players")]
    private GameObject Player1;
    private GameObject Player2;
    private PSC_PlayerController1 P1;
    private PSC_PlayerController2 P2;
    private Animator anim1;
    private Animator anim2;

    private void Awake()
    {
        Player1 = GameObject.Find("SK_Player1");
        Player2 = GameObject.Find("SK_Player2");
        P1 = Player1.GetComponent<PSC_PlayerController1>();
        P2 = Player2.GetComponent<PSC_PlayerController2>();
        anim1 = Player1.GetComponent<Animator>();
        anim2 = Player2.GetComponent<Animator>();
        image = GameObject.Find("Life").GetComponent<Image>();
        GO = GameObject.Find("GameOver").GetComponent<Image>();
        manager = GetComponent<MAN_SceneManager>();
    }

    private void Update()
    {
        image.fillAmount = Health;
        if (Health <= 0 && !Dcount)
        {
            Dcount = true;
            anim1.SetTrigger("GameOver");
            anim2.SetTrigger("GameOver");
            soundC.PlayGO();
            manager.gameover();
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
