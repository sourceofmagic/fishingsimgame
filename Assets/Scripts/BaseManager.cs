using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;

public class BaseManager : MonoBehaviour
{
    public GameObject bestiary;
    public GameObject lastStats;
    public GameObject totalStats;
    public GameObject upgradeTree;

    public GameObject bestiaryText;
    public GameObject lastStatsText;
    public GameObject totalStatsText;
    public GameObject upgradeTreeText;

    public enum WindowType { Bestiary, LastStats, TotalStats, UpgradeTree, Nothing };
    public WindowType currentWindow;

    private void Start()
    {
        OpenLastStats();
        CalculateLastStats();
    }

    public void ChangeWindow(WindowType windowType)
    {
        if (currentWindow == windowType) return;
        currentWindow = windowType;

        switch (windowType)
        {
            case WindowType.Bestiary:
                currentWindow = WindowType.Bestiary;
                bestiary.SetActive(true);
                lastStats.SetActive(false);
                totalStats.SetActive(false);
                upgradeTree.SetActive(false);
                break;
            case WindowType.LastStats:
                currentWindow = WindowType.LastStats;
                lastStats.SetActive(true);
                bestiary.SetActive(false);
                totalStats.SetActive(false);
                upgradeTree.SetActive(false);
                break;
            case WindowType.TotalStats:
                currentWindow = WindowType.TotalStats;
                totalStats.SetActive(true);
                bestiary.SetActive(false);
                lastStats.SetActive(false);
                upgradeTree.SetActive(false);
                break;
            case WindowType.UpgradeTree:
                currentWindow = WindowType.UpgradeTree;
                upgradeTree.SetActive(true);
                bestiary.SetActive(false);
                lastStats.SetActive(false);
                totalStats.SetActive(false);
                break;
        }
    }

    public void OpenBestiary()
    {
        ChangeWindow(WindowType.Bestiary);
    }

    public void OpenLastStats()
    {
        
        ChangeWindow(WindowType.LastStats);
    }

    public void OpenTotalStats()
    {
        ChangeWindow(WindowType.TotalStats);
    }

    public void OpenUpgradeTree()
    {
        ChangeWindow(WindowType.UpgradeTree);
    }

    public void CalculateLastStats()
    {
        lastStatsText.GetComponent<TextMeshProUGUI>().text = $"Рыб поймано: {GameManager.Instance.getFishAmount(GameManager.Instance.currentFishingResult)}" +
            $"\nТоплива потрачено: {GameManager.Instance.boatFuelTankVolume * 100}" +
            $"\nМонет получено: {GameManager.Instance.coins}" +
            $"\nНовых рыб найдено:{0}" +
            $"\nВремя в забеге: {0}";
    }

    public void StartGame()
    {
        GameManager.Instance.currentFishingResult.Clear();
        Debug.Log("Очищен временный словарь");
        SceneManager.LoadScene("planet1");

        Cursor.visible = false;
        InputManager.Instance?.ChangeInputMap(InputManager.InputType.Player);
    }
}
