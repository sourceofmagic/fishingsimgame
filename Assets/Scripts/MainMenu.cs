using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public bool isPause = false;
    private InputManager inputManager;

    private void Awake()
    {
        inputManager = InputManager.Instance;
    }

    private void OnEnable()
    {
        if(inputManager?.actions?.UI != null)
        {
            inputManager.actions.UI.ToPause.performed += OnPausePerformed;
        }
        ProgressBar.OnFuelExpiration += EndRun;
    }

    private void OnDisable()
    {
        if (inputManager?.actions?.UI != null)
        {
            inputManager.actions.UI.ToPause.performed -= OnPausePerformed;
        }
        ProgressBar.OnFuelExpiration -= EndRun;
    }

    private void OnPausePerformed(InputAction.CallbackContext ctx)
    {
        if (isPause)
            ReturnTOGame();
        else
            PauseGame();
    }
    public void StartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("planet1");

        Cursor.visible = false;
        inputManager?.ChangeInputMap(InputManager.InputType.Player);
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPause = true;
        Cursor.visible = true;

        inputManager?.ChangeInputMap(InputManager.InputType.UI);
    }
    
    public void ToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Cursor.visible = true;
        Time.timeScale = 1f;
        inputManager?.ChangeInputMap(InputManager.InputType.UI);

        GameManager.Instance.currentFishingResult.Clear();
    }

    public void ReturnTOGame()
    {
        if (isPause)
        {
            pauseMenu.SetActive(false);
            isPause = false;
            Time.timeScale = 1f;
            Cursor.visible = false;

            inputManager?.ChangeInputMap(InputManager.Instance.lastMap);
        }
        
    }

    public void EndRun()
    {
        SceneManager.LoadScene("Base");
        Cursor.visible = true;
        inputManager?.ChangeInputMap(InputManager.InputType.UI);
    }

    public void ExitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}