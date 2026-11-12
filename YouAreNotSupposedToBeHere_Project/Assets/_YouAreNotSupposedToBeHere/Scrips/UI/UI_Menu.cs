using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Menu : MonoBehaviour
{

    public void Play()
    {
        Scene Actual = SceneManager.GetActiveScene();
        int NextIndex = Actual.buildIndex + 1;
        SceneManager.LoadScene(NextIndex);
    }

    public void Exit()
    {
        Application.Quit();
    }

}
