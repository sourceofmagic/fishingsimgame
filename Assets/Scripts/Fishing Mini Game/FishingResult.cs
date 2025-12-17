using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FishingResult : MonoBehaviour
{
    public static FishingResult Instance { get; set; }

    public GameObject resultText;
    public GameObject fishSprite;
    public GameObject result;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    internal void ResultInfo(FishData fish)
    {
        result.gameObject.SetActive(true);
        resultText.GetComponent<TextMeshProUGUI>().text = "Поймано: " + fish.fishName;
        fishSprite.GetComponent<Image>().sprite = fish.inventoryItem;
        StartCoroutine(CloseResultInfo());
        Debug.LogWarning("Рыба попала в функцию " + fish.fishName);
    }

    IEnumerator CloseResultInfo()
    {
        yield return new WaitForSeconds(4f);

        result.gameObject.SetActive(false);
        Debug.LogWarning("Отключили панель");
    }
}
