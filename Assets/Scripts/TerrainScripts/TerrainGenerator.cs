using UnityEngine;
using UnityEngine.TerrainUtils;

[RequireComponent(typeof(Terrain))]
public class TerrainGenerator : MonoBehaviour
{
    [Header("Настройки генерации")]
    public int seed = 12345;

    [Tooltip("Масштаб шума. Меньше значение — больше гор.")]
    public float noiseScale = 20f;

    [Tooltip("Множитель высоты. Определяет максимальную высоту гор.")]
    public float heightMultiplier = 20f;

    [Tooltip("Смещение по X и Y, чтобы разные сиды давали разный результат.")]
    public Vector2 noiseOffset = new Vector2(100f, 100f);

    private Terrain terrain;
    private TerrainData terrainData;

    private void Start()
    {
        terrain = GetComponent<Terrain>();
        terrainData = terrain.terrainData;

        if (seed != 0)
            noiseOffset = new Vector2(seed * 0.123f, seed * 0.456f);


        GenerateTerrain();
    }

    public void GenerateTerrain()
    {
        int width = terrainData.heightmapResolution;
        int height = terrainData.heightmapResolution;

        float[,] heights = new float[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float xCoord = (float)x / width * noiseScale + noiseOffset.x;
                float yCoord = (float)y / height * noiseScale + noiseOffset.y;

                float perlinValue = Mathf.PerlinNoise(xCoord, yCoord);
                heights[x, y] = perlinValue * (heightMultiplier / terrainData.size.y);
            }
        }
        terrainData.SetHeights(0, 0, heights);

        terrain.Flush();
    }
}
