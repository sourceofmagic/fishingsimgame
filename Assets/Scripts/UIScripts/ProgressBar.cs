using UnityEngine;
using UnityEngine.UI;
using System;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Image fuel;
    private float currentFuelAmount;

    public static event Action OnFuelExpiration;

    private float startTime;
    private float finishTime;

    private void Start()
    {
        fuel = gameObject.GetComponent<Image>();
        if (fuel.type != Image.Type.Filled)
        {
            Debug.LogError("Image should be type Filled");
        }
        startTime = Time.realtimeSinceStartup;
    }

    private void OnEnable()
    {
        FishingMiniGame.OnFishingEnd += ChangeFuelAmount;
    }
    private void OnDisable()
    {
        FishingMiniGame.OnFishingEnd -= ChangeFuelAmount;
    }

    public void ChangeFuelAmount()
    {
        Debug.Log("Меняем прогрессбар");
        currentFuelAmount = fuel.fillAmount;
        float speed = GameManager.Instance.fuelConsumption;
        if (currentFuelAmount - speed < 0)
        { 
            fuel.fillAmount = 0;
        }
        else
            fuel.fillAmount -= speed;

        if (fuel.fillAmount == 0)
        { 
            OnFuelExpiration?.Invoke();
            finishTime = Time.realtimeSinceStartup;
            GameManager.Instance.timeInSwim = finishTime - startTime;
        }
    }
}
