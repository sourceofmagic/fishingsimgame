using UnityEngine;
using UnityEngine.UI;

public class MiniGameRealization : MonoBehaviour
{
    public RectTransform fishTransform;
    public RectTransform cursorTransform;

    public bool isFishOverlapping; // перекрытие

    public Slider successSlider;
    float successIncrement = 15;
    float failDecrement = 12;
    float successThreshold = 100;  // порог
    float failThreshold = -100;
    float successCounter = 0;

    private void Update()
    {
        if(CheckOverlapping(fishTransform,  cursorTransform)) 
        {
            isFishOverlapping = true;
        }
        else
        {
            isFishOverlapping = false;
        }

        OverlappingCalculation();
    }

    private bool CheckOverlapping(RectTransform rect1, RectTransform rect2)
    {
        Rect r1 = new Rect(rect1.position.x, rect1.position.y, rect1.rect.width, rect1.rect.height);
        Rect r2 = new Rect(rect2.position.x, rect2.position.y, rect2.rect.width, rect2.rect.height);
        return r1.Overlaps(r2);
    }

    private void OverlappingCalculation()
    {
        if (isFishOverlapping)
        {
            successCounter += successIncrement * Time.deltaTime;
        }
        else
        {
            successCounter -= failDecrement * Time.deltaTime;
        }

        successCounter = Mathf.Clamp(successCounter, failThreshold, successThreshold);

        successSlider.value = successCounter;

        if(successCounter >= successThreshold)
        {
            Debug.Log("succeess");
            FishingMiniGame.Instance.EndMiniGame(true);
            successSlider.value = 0;
            successCounter = 0;
        }
        else if(successCounter <= failThreshold)
        {
            Debug.Log("failed");
            FishingMiniGame.Instance.EndMiniGame(false);
            successSlider.value = 0;
            successCounter = 0;
        }
    }
}
