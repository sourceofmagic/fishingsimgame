using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.TerrainUtils;

[RequireComponent(typeof(Terrain))]
public class TerrainGenerator : MonoBehaviour
{
    public TerrainGenerationConfig config;

    public bool generateOnStart = true;

    private Terrain terrain;
    private TerrainData terrainData;

    private void Start()
    {
        Initialize();

        if (generateOnStart)
            GenerateTerrain();
    }

    public void GenerateTerrain()
    {

        Initialize();

        if (config == null)
        {
            Debug.LogError($"[TerrainGenerator] Конфиг не назначен на объекте {gameObject.name}!", this);
            return;
        }

        int effectiveSeed = config.GetEffectiveSeed();

        // Вычисляем смещение на основе сида, если пользователь не задал его вручную
        Vector2 offset = config.noiseOffset;
        if (config.useRandomSeed || effectiveSeed != 0)
        {
            // Детерминированное смещение из сида
            offset = new Vector2(
                (effectiveSeed % 10000) * 0.123f,
                (effectiveSeed % 10000) * 0.456f
            );
        }

        int width = terrainData.heightmapResolution;
        int height = terrainData.heightmapResolution;

        float[,] heights = new float[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float xCoord = (float)x / width * config.noiseScale + config.noiseOffset.x;
                float yCoord = (float)y / height * config.noiseScale + config.noiseOffset.y;

                float perlinValue = GetFractalNoise(xCoord, yCoord);

                // --- НОВЫЙ КОД: Falloff (Низина в центре) ---
                if (config.useFalloff)
                {
                    // Находим нормализованные координаты от 0 до 1
                    float nx = (float)x / width;
                    float ny = (float)y / height;

                    // Вычисляем расстояние от центра (0.5, 0.5). 
                    // Умножаем на 2, чтобы максимальное расстояние было равно 1.
                    float distanceFromCenter = Vector2.Distance(new Vector2(nx, ny), new Vector2(0.5f, 0.5f)) * 2f;

                    // Возводим в степень для контроля резкости и умножаем на силу
                    float falloff = Mathf.Pow(distanceFromCenter, config.falloffPower) * config.falloffStrength;

                    // Вычитаем falloff из высоты шума. Ближе к краям высота будет расти.
                    perlinValue = perlinValue - falloff;

                    // Не даем высоте уйти ниже нуля
                    perlinValue = Mathf.Clamp01(perlinValue);
                }
                // ---------------------------------------------

                // если нужна инверсия
                if (config.invertHeight)
                    perlinValue = 1f - perlinValue;

                heights[x, y] = perlinValue * (config.heightMultiplier / terrainData.size.y);

            }
        }
        terrainData.SetHeights(0, 0, heights);

        terrain.Flush();
    }

    private float GetFractalNoise(float x, float y)
    {
        float total = 0f;
        float frequency = 1f;
        float amplitude = 1f;
        float maxValue = 0f; // Нужно для нормализации результата в диапазон 0-1

        for (int i = 0; i < config.octaves; i++)
        {
            // Считаем шум Перлина для текущей октавы
            float noiseValue = Mathf.PerlinNoise(x * frequency, y * frequency);

            // Добавляем к общему результату с учетом текущей амплитуды
            total += noiseValue * amplitude;
            maxValue += amplitude;

            // Увеличиваем частоту и уменьшаем амплитуду для следующей октавы
            frequency *= config.lacunarity;
            amplitude *= config.persistence;
        }

        // Делим на максимально возможную сумму, чтобы результат был от 0 до 1
        return total / maxValue;
    }

    private void Initialize()
    {
        if (terrain == null) terrain = GetComponent<Terrain>();
        if (terrainData == null) terrainData = terrain.terrainData;
    }
}
