using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public List<FishData> fishList;


    public Dictionary<FishData, int> currentFishingResult;
    public Dictionary<FishData, int> fishDictionary;
    public float boatFuelTankVolume = 1.0f;
    public float fuelConsumption = 0.25f;
    public int coins = 0;


    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Init()
    {
        if (Instance != null) return;

        GameObject go = new GameObject("GameManager");
        Instance = go.AddComponent<GameManager>();
        DontDestroyOnLoad(go);
    }
    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        fishList = new List<FishData>();
        currentFishingResult = new Dictionary<FishData, int>();
        fishDictionary = new Dictionary<FishData, int>();

        LoadAllFishData("FishData");

        if (fishList != null)
        {
            foreach (FishData fish in fishList)
            {
                fishDictionary[fish] = 0;
            }
        }

        ProgressBar.OnFuelExpiration += ApplyResultsOfFishing;
    }

    // инициализация списка
    public void LoadAllFishData(string folderPath)
    {
        FishData[] loadedFish = Resources.LoadAll<FishData>(folderPath);
        fishList.Clear();
        fishList.AddRange(loadedFish);

        if (fishList != null) Debug.Log("Список рыб проинициализирован");
        else Debug.LogWarning("Список рыб не получен");
    }

    // Заносит пойманую рыбу во временное хранилище
    public void CaughtFish(FishData fishdata)
    {
        if (currentFishingResult.ContainsKey(fishdata))
        {
            currentFishingResult[fishdata] += 1;
        }
        else
        {
            currentFishingResult[fishdata] = 1;
        }

    }

    // Принимаем результаты заплыва в общий счётчик
    public void ApplyResultsOfFishing()
    {
        foreach(KeyValuePair<FishData, int> fish in currentFishingResult)
        {
            // считаем монетки
            if (fish.Key.fishName == "Ничего")
                coins += 0;
            else if (fish.Key.probability == 25)
                coins += 1;
            else if (fish.Key.probability > 1)
                coins += 3;
            else
                coins += 100;

            // заносим в общий счётчик
            fishDictionary[fish.Key] += fish.Value;
        }
    }

    // Для прокачки траты топлива
    public void DecreaseFuelConsumption(int x)
    {
        if (x >= fuelConsumption)
        { fuelConsumption = 0; return; }

        fuelConsumption -= x;
    }

    // Для прокачки вместимости бака с топливом
    public void IncreaseFuelVolume(int x)
    {
        if (x > 1000)
        { boatFuelTankVolume = 1000; return; }

        boatFuelTankVolume += x;
    }

    // количество вылова за текущий забег
    public int getFishAmount(Dictionary<FishData, int> dict)
    {
        int count = 0;
        if (dict != null)
            foreach (KeyValuePair<FishData, int> fish in currentFishingResult)
                count += fish.Value;
        return count;
    }
}
