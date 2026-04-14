using UnityEngine;

public class WheelTerrainPainter : MonoBehaviour
{
    public Terrain terrain;
    public int textureIndex = 1; // индекс текстуры грязи в Terrain Layer
    public float brushSize = 1.0f;
    public float brushStrength = 0.5f;

    private TerrainData terrainData;

    void Start()
    {
        if (terrain == null)
            terrain = Terrain.activeTerrain;

        terrainData = terrain.terrainData;
    }

    void Update()
    {
        PaintUnderWheel();
    }

    void PaintUnderWheel()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, 2f))
        {
            Vector3 terrainPos = hit.point - terrain.transform.position;

            int mapX = (int)((terrainPos.x / terrainData.size.x) * terrainData.alphamapWidth);
            int mapZ = (int)((terrainPos.z / terrainData.size.z) * terrainData.alphamapHeight);

            int size = Mathf.RoundToInt(brushSize);

            float[,,] splat = terrainData.GetAlphamaps(mapX, mapZ, size, size);

            for (int x = 0; x < size; x++)
            {
                for (int z = 0; z < size; z++)
                {
                    // уменьшаем траву
                    splat[x, z, 0] *= (1f - brushStrength);

                    // добавляем грязь
                    splat[x, z, textureIndex] += brushStrength;
                }
            }

            terrainData.SetAlphamaps(mapX, mapZ, splat);
        }
    }
}
