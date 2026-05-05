using TMPro;
using UnityEngine;

public class TotalStatsManager : MonoBehaviour
{
    public TextMeshProUGUI body;

    GameManager gm;

    private void Awake()
    {
        gm = GameManager.Instance;
    }

    private void OnEnable()
    {
        body.text = $"Всего рыб поймано: {gm.getFishAmount(gm.fishDictionary)}" +
            $"\nЗаработано монет: {gm.totalCoins}" +
            $"\nКоличество заплывов: {gm.swimCount}" +
            $"\nОбщее время заплывов: {gm.FormatTime(gm.totalTimeInSwim)}";
    }

    private void OnDisable()
    {
        body.text = string.Empty;
    }
}
