using UnityEngine;

[CreateAssetMenu(fileName = "FishData", menuName = "FishData", order = 1 )]
public class FishData : ScriptableObject
{
    public string fishName;
    public GameObject inventoryItem;
    public int probability;
}
