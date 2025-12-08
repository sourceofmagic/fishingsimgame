using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FishingMiniGame : MonoBehaviour
{
    public static FishingMiniGame Instance { get; set; }

    public List<FishData> fishList; // список возможных рыбов

    public bool isThereABite;
    public bool hasPulled;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    internal void StartFishing()
    {
        StartCoroutine(FishingCoroutine());
    }

    IEnumerator FishingCoroutine()
    {
        yield return new WaitForSeconds(4f);

        FishData fish = CalculateBite();

        if (fish.fishName == "Ничего")
        {
            Debug.LogWarning("Вы поймали что-то склизкое и мерзкое, это точно не рыба");
            EndFishing();

        }
        else
        {
            Debug.LogWarning(fish.fishName + " на крючке!");
            StartCoroutine(StartFishStruggle(fish));
        }
    }

    IEnumerator StartFishStruggle(FishData fish)
    {
        isThereABite = true;

        // ждем пока игрок начнет рыбачить

        while(!hasPulled)
        {
            yield return null;
        }
        Debug.LogWarning("Мини-игра!");
    }

    public void setHasPulled()
    {
        hasPulled = true;
    }

    private void EndFishing()
    {
        isThereABite = false;
        hasPulled = false;
    }

    private FishData CalculateBite()
    {
        // Расчитываем общую вероятность
        float totalProbability = 0f;
        foreach(FishData fish in fishList)
        {
            totalProbability += fish.probability;
        }

        // Генерируем рандом число от 0 до общей вероятности
        int randomValue = UnityEngine.Random.Range(0, Mathf.FloorToInt(totalProbability) + 1);
        Debug.Log("Рандом число: " + randomValue);

        float cumulativeProbability = 0f;
        foreach (FishData fish in fishList)
        {
            cumulativeProbability += fish.probability;
            if(randomValue <= cumulativeProbability)
            {
                return fish;
            }
        }
        return null;
    }
}
