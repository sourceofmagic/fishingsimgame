using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BestiaryManager : MonoBehaviour
{
    InputManager inputManager;
    List<FishData> fishList;
    int page;

    FishData currentFish;
    Vector2 move;

    public Image fishSprite;
    public TextMeshProUGUI fishName;
    public TextMeshProUGUI fishDescription;
    public TextMeshProUGUI fishCount;

    public void Start()
    {
        //ЧИТЫ!!
        //GameManager.Instance.AddManyFish();

        inputManager = InputManager.Instance;
        InitializeBestiary();
    }
    public void OnEnable()
    {
        if (inputManager?.actions?.UI != null)
        {
            inputManager.actions.UI.Navigate.performed += OnBestiaryScroll;
        }
    }

    public void OnDisable()
    {
        if (inputManager?.actions?.UI != null)
        {
            inputManager.actions.UI.Navigate.performed -= OnBestiaryScroll;
        }
    }

    public void OnBestiaryScroll(InputAction.CallbackContext ctx)
    { 
        move = ctx.ReadValue<Vector2>();
        if(fishList.Count > 1)
            ScrollFish(); 
    }

    public void ScrollFish()
    {
        if (move.x > 0 && page + 1 >= fishList.Count)
            return;
        else if (move.x < 0 && page - 1 < 0)
            return;

        page += move.x > 0 ? 1 : -1;
        currentFish = fishList[page];
        fishSprite.sprite = currentFish.inventoryItem;
        fishName.text = currentFish.fishName;
        fishDescription.text = currentFish.description;
        fishCount.text = $"Поймано всего: {GameManager.Instance.fishDictionary[currentFish]}";

    }

    public void InitializeBestiary()
    {
        fishList = new List<FishData>();
        foreach (KeyValuePair<FishData, int> fish in GameManager.Instance.fishDictionary)
        {
            if (fish.Value != 0 && !fishList.Contains(fish.Key))
                fishList.Add(fish.Key);
        }
        if (fishList.Count > 0)
        {
            currentFish = fishList[0];
            fishSprite.sprite = currentFish.inventoryItem;
            fishName.text = currentFish.fishName;
            fishDescription.text = currentFish.description;
            fishCount.text = $"Поймано всего: { GameManager.Instance.fishDictionary[currentFish] }";

            page = 0;
        }
        else
        {
            fishDescription.text = "Бестиарий пуст";
        }
    }
}
