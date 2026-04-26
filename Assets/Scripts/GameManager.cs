using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public List<FishData> fishList;

    public int fishCountTest;

    private Dictionary<FishData, int> currentFishingResult;
    private Dictionary<FishData, int> fishDictionary;
    [SerializeField] private int boatFuelTankVolume = 100;
    [SerializeField] private int fuelConsumption = 20;
    [SerializeField] private int coins;


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

        fishCountTest = getCurrentFishAmount();

    }

    // Принимаем результаты заплыва в общий счётчик
    public void ApplyResultsOfFishing()
    {
        foreach(KeyValuePair<FishData, int> fish in currentFishingResult)
        {
            fishDictionary[fish.Key] += fish.Value;
        }
        currentFishingResult.Clear();
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
    public int getCurrentFishAmount()
    {
        int count = 0;
        if (currentFishingResult != null)
            foreach (KeyValuePair<FishData, int> fish in currentFishingResult)
                count += fish.Value;
        return count;
    }

    public int getTotalFishAmount()
    {
        int count = 0;
        if (fishDictionary != null)
            foreach (KeyValuePair<FishData, int> fish in fishDictionary)
                count += fish.Value;
        return count;
    }
}
