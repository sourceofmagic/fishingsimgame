using UnityEngine;

[CreateAssetMenu(fileName = "TerrainGenerationConfig", menuName = "Scriptable Objects/TerrainGenerationConfig")]
public class TerrainGenerationConfig : ScriptableObject
{
    [Header("Сид и воспроизводимость")]
    [Tooltip("Если 0 — используется случайный сид. Иначе — детерминированный.")]

    public int seed = 12345;
    public bool useRandomSeed = false;
    public bool invertHeight = false;

    [Header("Шум Перлина")]
    [Tooltip("Масштаб шума. Меньше — крупнее горы, больше — мельче детали.")]

    [Range(1f, 200f)]
    public float noiseScale = 20f;

    [Tooltip("Мнгожитель высоты в мировых единицах.")]
    [Range(0f, 200f)]
    public float heightMultiplier = 20f;

    [Header("Смещение")]
    [Tooltip("Смещение координат шума. Позволяет исследовать разные области шума.")]
    public Vector2 noiseOffset = new Vector2(100f, 100f);

    // =============================================================================================

    [Header("Фрактальный шум (fBm)")]
    [Tooltip("Количество слоев шума. Больше = больше деталей.")]
    [Range(1, 8)]
    public int octaves = 4;

    [Tooltip("Как быстро уменьшается частота. Обычно 2.0.")]
    [Range(1f, 4f)]
    public float lacunarity = 2f;

    [Tooltip("Как быстро уменьшается амплитуда. Обычно 0.5.")]
    [Range(0f, 1f)]
    public float persistence = 0.5f;

    // =============================================================================================

    [Header("Falloff (Низина в центре)")]
    [Tooltip("Включить/выключить эффект низины.")]
    public bool useFalloff = false;

    [Tooltip("Сила эффекта. Чем больше, тем выше края.")]
    [Range(0.1f, 5f)]
    public float falloffStrength = 1.5f;

    [Tooltip("Резкость перехода от низины к горам. Больше = резче.")]
    [Range(1f, 10f)]
    public float falloffPower = 3f;

    // ==============================================================================================

    // Метод для получения финального сида с учётом флага useRandomSeed
    public int GetEffectiveSeed()
    {
        if (useRandomSeed)
        {
            return Random.Range(int.MinValue, int.MaxValue);
        }
        return seed;
    }
}
