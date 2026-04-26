using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseManager : MonoBehaviour
{
    public GameObject bestiary;
    public GameObject lastStats;
    public GameObject totalStats;
    public GameObject upgradeTree;

    public bool isBestiaryActive = false;
    public bool isLastStatsActive = true;
    public bool isTotalStatsActive = false;
    public bool isUpgradeTreeActive = false;

    public void ChangeBestiaryVisibility()
    {
        isBestiaryActive = !isBestiaryActive;
        bestiary.SetActive(isBestiaryActive);
    }

    public void ChangeLastStatsVisibility()
    {
        isLastStatsActive = !isLastStatsActive;
        lastStats.SetActive(isLastStatsActive);
    }

    public void ChangeTotalStatsVisibility()
    {
        isTotalStatsActive = !isTotalStatsActive;
        totalStats.SetActive(isTotalStatsActive);
    }

    public void ChangeUpgradeTreeVisibility()
    {
        isUpgradeTreeActive = !isUpgradeTreeActive;
        upgradeTree.SetActive(isUpgradeTreeActive);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("planet1");

        Cursor.visible = false;
        InputManager.Instance?.ChangeInputMap(InputManager.InputType.Player);
    }
}
