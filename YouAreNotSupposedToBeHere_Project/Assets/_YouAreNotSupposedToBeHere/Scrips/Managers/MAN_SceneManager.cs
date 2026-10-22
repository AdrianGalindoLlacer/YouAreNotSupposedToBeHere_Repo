using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MAN_SceneManager : MonoBehaviour
{
    [SerializeField] PSC_PlayerController1 P1;
    [SerializeField] PSC_PlayerController2 P2;
    private bool Paused = false;

    public void Restart()
    {
        Scene Actual = SceneManager.GetActiveScene();
        SceneManager.LoadScene(Actual.name);
    }
    public void NextScene()
    {
        Scene Actual = SceneManager.GetActiveScene();
        int NextIndex = Actual.buildIndex + 1;
        SceneManager.LoadScene(NextIndex);
    }

    public void pause()
    {
        Time.timeScale = 0;
        P1.enabled = false;
        P2.enabled = false;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        Paused = true;
    }

    public void Resume()
    {
        Time.timeScale = 1;
        P1.enabled = true;
        P2.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Paused = false;
    }

    #region Input Methods
    public void OnPause(InputAction.CallbackContext context)
    {
        if (Paused) pause();
        else Resume();
    }
    #endregion
}
