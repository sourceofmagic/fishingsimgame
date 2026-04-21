using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public GameObject pauseMenu;
    public void StartGame()
    {
        SceneManager.LoadScene("planet1");
    }
    
    public void ToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Cursor.visible = true;
    }

    public void ReturnTOGame()
    {
        pauseMenu.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}