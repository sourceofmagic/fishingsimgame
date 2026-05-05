using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class UpgradeTreeScript : MonoBehaviour
{
    GameManager gm;

    public GameObject successPopup;

    [Header("Удача")]
    public Button[] luckButtons;
    [Header("Корабль")]
    public Button[] shipButtons;
    [Header("Инструменты")]
    public Button[] toolsButtons;

    private void Start()
    {
        gm = GameManager.Instance;
    }

    private void OnEnable()
    {
        RefreshButtons();
    }
    private void RefreshButtons()
    {
        for (int i = 0; i < 3; i++)
        {
            luckButtons[i].interactable = !gm.luck[i];
            luckButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = $"{gm.luckPrices[i]}";
            shipButtons[i].interactable = !gm.ship[i];
            shipButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = $"{gm.shipPrices[i]}";
            toolsButtons[i].interactable = !gm.tools[i];
            toolsButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = $"{gm.toolsPrices[i]}";
        }


    }

    public void OnLuckUpgradeClick(int upgradeIndex)
    {
        TryUpgrade(0, upgradeIndex, luckButtons[upgradeIndex]);
    }

    public void OnShipUpgradeClick(int upgradeIndex)
    {
        TryUpgrade(1, upgradeIndex, shipButtons[upgradeIndex]);
    }

    public void OnToolsUpgradeClick(int upgradeIndex)
    {
        TryUpgrade(2, upgradeIndex, toolsButtons[upgradeIndex]);
    }

    public void TryUpgrade(int category, int index, Button button)
    {
        switch (category) {
            case 0:
                if (!gm.luck[index] && gm.luckPrices[index] <= gm.totalCoins)
                {
                    button.interactable = false;
                    gm.totalCoins -= gm.luckPrices[index];
                    gm.luck[index] = true;
                    // Показываем окно успеха
                    StartCoroutine(ShowPopup("Повышена удача"));
                }
                break;
            case 1:
                if (!gm.ship[index] && gm.shipPrices[index] <= gm.totalCoins)
                {
                    button.interactable = false;
                    gm.totalCoins -= gm.shipPrices[index];
                    gm.ship[index] = true;
                    // Показываем окно успеха
                    StartCoroutine(ShowPopup("Разблокировано усиление корабля"));
                }
                break;
            case 2:
                if (!gm.tools[index] && gm.toolsPrices[index] <= gm.totalCoins)
                {
                    button.interactable = false;
                    gm.totalCoins -= gm.toolsPrices[index];
                    gm.tools[index] = true;
                    // Показываем окно успеха
                    StartCoroutine(ShowPopup("Разблокировано усиление инструментов"));
                }
                break;
        }
    }

    public IEnumerator ShowPopup(string info)
    {
        successPopup.SetActive(true);
        successPopup.GetComponent<TextMeshProUGUI>().text = info;
        yield return new WaitForSeconds(4f);
        successPopup.SetActive(false);
    }
}
