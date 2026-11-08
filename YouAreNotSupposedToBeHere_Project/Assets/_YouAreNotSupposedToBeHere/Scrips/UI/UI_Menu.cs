using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Menu : MonoBehaviour
{

    public void Play3()
    {
        SceneManager.LoadScene("SCE_Level3");
    }

    public void Exit()
    {
        Application.Quit();
    }

}
