using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;
using static UnityEditor.Experimental.GraphView.GraphView;

public class MAN_SceneManager : MonoBehaviour
{
    [Header("Pause")]
    [SerializeField] GameObject PauseMenu;
    [SerializeField] GameObject UI;
    private bool Paused = false;
    private string exit = "SCE_Menu";

    [Header("Referencias Players")]
    private GameObject P1;
    private GameObject P2;
    private PSC_PlayerController1 P1Controller;
    private PSC_PlayerController2 P2Controller;
    private PSC_PlayerShoot1 P1Shoot;
    private PSC_PlayerShoot2 P2Shoot;

    [Header("GameOver")]
    [SerializeField] CanvasGroup GameoverCanva;
    [SerializeField] float GameFadeIn = 1f;
    [SerializeField] float GameOverDuration = 1f;
    private float GOtime = 0f;
    private float GOduration = 0f;
    private int count;

    private void Awake()
    {
        P1 = GameObject.Find("SK_Player1");
        P2 = GameObject.Find("SK_Player2");
        P1Controller = P1.GetComponent<PSC_PlayerController1>();
        P2Controller = P2.GetComponent<PSC_PlayerController2>();
        P1Shoot = P1.GetComponent<PSC_PlayerShoot1>();
        P2Shoot = P2.GetComponent<PSC_PlayerShoot2>();
    }


    public void Restart()
    {
        Scene Actual = SceneManager.GetActiveScene();
        SceneManager.LoadScene(Actual.name);
        Time.timeScale = 1;
        P1Controller.enabled = true;
        P2Controller.enabled = true;
    }
    public void NextScene()
    {
        Scene Actual = SceneManager.GetActiveScene();
        int NextIndex = Actual.buildIndex + 1;
        SceneManager.LoadScene(NextIndex);
    }

    public void Pause()
    {
        PauseMenu.SetActive(true);
        UI.SetActive(false);
        Time.timeScale = 0;
        P1Controller.enabled = false;
        P2Controller.enabled = false;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        Paused = true;
    }

    public void gameover()
    {
        P1Controller.Block = false;
        P2Controller.Block = false;
        P1Controller.enabled = false;
        P2Controller.enabled = false;
        StartCoroutine(GameOverFade());
    }

    private IEnumerator GameOverFade()
    {
        GameoverCanva.interactable = true;
        GameoverCanva.blocksRaycasts = true;


        while (GOtime < GameFadeIn)
        {
            GOtime += Time.deltaTime;
            GameoverCanva.alpha = Mathf.Clamp01(GOtime / GameFadeIn);
            yield return null;
        }
        while (GOduration < GameOverDuration)
        {
            GOduration += Time.deltaTime;
            yield return null;
        }
        Restart();
    }

    public void Resume()
    {
        PauseMenu.SetActive(false);
        UI.SetActive(true);
        Time.timeScale = 1;
        P1Controller.enabled = true;
        P2Controller.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Paused = false;
    }

    public void Exit()
    {
        SceneManager.LoadScene(exit);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            count++;
            if (count == 2 && P1Shoot.HoldRobot)
            {
                NextScene();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            count--;
            if(count <= 0)
            {
                count = 0;
            }
        }
    }
    #region Input Methods
    public void OnPause(InputAction.CallbackContext context)
    {
        if (!Paused) Pause();
        else Resume();
    }
    #endregion
}
