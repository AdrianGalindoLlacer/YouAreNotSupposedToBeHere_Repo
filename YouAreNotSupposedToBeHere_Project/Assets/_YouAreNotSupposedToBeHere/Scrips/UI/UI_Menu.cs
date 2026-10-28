using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Menu : MonoBehaviour
{

    public void play3()
    {
        SceneManager.LoadScene("SCE_Level3");
    }
    public void play4()
    {
        SceneManager.LoadScene("SCE_Level4");
    }

    public void exit()
    {
        Application.Quit();
    }

}
