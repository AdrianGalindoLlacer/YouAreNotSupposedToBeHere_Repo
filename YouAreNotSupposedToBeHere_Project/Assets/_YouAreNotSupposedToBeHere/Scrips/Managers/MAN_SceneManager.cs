using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MAN_SceneManager : MonoBehaviour
{
    [SerializeField] PSC_PlayerController1 P1;
    [SerializeField] PSC_PlayerController2 P2;
    [SerializeField] GameObject PauseMenu;
    [SerializeField] GameObject UI;
    private bool Paused = false;
    private string exit = "SCE_Menu";



    public void Restart()
    {
        Scene Actual = SceneManager.GetActiveScene();
        SceneManager.LoadScene(Actual.name);
        Time.timeScale = 1;
        P1.enabled = true;
        P2.enabled = true;
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
        P1.enabled = false;
        P2.enabled = false;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        Paused = true;
    }

    public void Resume()
    {
        PauseMenu.SetActive(false);
        UI.SetActive(true);
        Time.timeScale = 1;
        P1.enabled = true;
        P2.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Paused = false;
    }

    public void Exit()
    {
        SceneManager.LoadScene(exit);
    }

    #region Input Methods
    public void OnPause(InputAction.CallbackContext context)
    {
        if (!Paused) Pause();
        else Resume();
    }
    #endregion
}
