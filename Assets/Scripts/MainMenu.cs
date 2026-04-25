using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public bool isPause = false;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.CapsLock))
            PauseGame();
    }
    public void StartGame()
    {
        SceneManager.LoadScene("planet1");
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPause = true;
        Cursor.visible = true;
    }
    
    public void ToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Cursor.visible = true;
        Time.timeScale = 1f;
    }

    public void ReturnTOGame()
    {
        if (isPause)
        {
            pauseMenu.SetActive(false);
            isPause = false;
            Time.timeScale = 1f;
        }
        
    }

    public void ExitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}