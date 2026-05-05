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
    public List<FishData> newFishEncountered;

    public float boatFuelTankVolume = 1.0f;
    public float fuelConsumption = 0.25f;
    public int totalCoins = 0;
    public int newCoins = 0;

    public int swimCount = 0;

    public float timeInSwim;
    public float bestTimeInSwim;
    public float totalTimeInSwim;

    // upgrades
    public List<bool> luck;
    public List<bool> ship;
    public List<bool> tools;

    public List<int> luckPrices = new List<int> {10, 15, 50};
    public List<int> shipPrices = new List<int> {5, 10, 15};
    public List<int> toolsPrices = new List<int> {100, 200, 300};


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
        newFishEncountered = new List<FishData>();

        LoadAllFishData("FishData");

        if (fishList != null)
        {
            foreach (FishData fish in fishList)
            {
                fishDictionary[fish] = 0;
            }
        }

        luck = new List<bool> { false, false, false };
        ship = new List<bool> { false, false, false };
        tools = new List<bool> { false, false, false };

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
        newFishEncountered = new List<FishData>();

        foreach (KeyValuePair<FishData, int> fish in currentFishingResult)
        {
            // считаем монетки
            if (fish.Key.fishName == "Ничего")
                newCoins += 0;
            else if (fish.Key.probability == 25)
                newCoins += 1;
            else if (fish.Key.probability > 1)
                newCoins += 3;
            else
                newCoins += 100;

            // заносим в общий счётчик
            if (fishDictionary[fish.Key] == 0)
                newFishEncountered.Add(fish.Key);

            fishDictionary[fish.Key] += fish.Value;
        }

        if (timeInSwim < bestTimeInSwim)
            bestTimeInSwim = timeInSwim;

        totalTimeInSwim += timeInSwim;

        totalCoins += newCoins;
        swimCount++;
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

    // количество вылова из словаря
    public int getFishAmount(Dictionary<FishData, int> dict)
    {
        int count = 0;
        if (dict.Count > 0)
            foreach (KeyValuePair<FishData, int> fish in dict)
                count += fish.Value;
        return count;
    }

    // ЧИТЫ
    public void AddManyFish()
    {
        foreach (FishData fish in fishList)
            fishDictionary[fish] = 10;
        totalCoins = 100000;
        totalTimeInSwim = 100f;
    }

    // сброс всех счётчиков перед началом нового заплыва
    public void StartNewSwim()
    {
        currentFishingResult.Clear();
        timeInSwim = 0;
        newFishEncountered.Clear();
        newCoins = 0;

    }

    // Форматирование времени
    public string FormatTime(float seconds)
    {
        int mins = Mathf.FloorToInt(seconds / 60);
        int secs = Mathf.FloorToInt(seconds % 60);
        int millis = Mathf.FloorToInt((seconds * 100) % 100);
        return $"{mins:00}:{secs:00}:{millis:00}";
    }
}