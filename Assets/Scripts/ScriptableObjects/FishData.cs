using UnityEngine;

[CreateAssetMenu(fileName = "FishData", menuName = "FishData", order = 1 )]
public class FishData : ScriptableObject
{
    public string fishName;
    public Sprite inventoryItem;
    public int probability;
}
