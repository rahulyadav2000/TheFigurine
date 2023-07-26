using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    [SerializeField] private int height = 10;
    [SerializeField] private int width = 150;
    [SerializeField] private int length = 150;
    [SerializeField] private int scale = 20;
    [SerializeField] private float randX = 100;
    [SerializeField] private float randY = 100;

    public Terrain terrain;

    private void Start()
    {
        randX = Random.Range(0, width);
        randY = Random.Range(0, length);
    }
    private void Update()
    {
        terrain.terrainData = GenerateTerrain(terrain.terrainData);
        
    }

    TerrainData GenerateTerrain(TerrainData terrainData)
    {
        terrainData.heightmapResolution = width + 1;

        terrainData.size = new Vector3(width, height, length);
        terrainData.SetHeights(0, 0, GenerateHeights());

        return terrainData;
    }

    float[,] GenerateHeights()
    {
        float[,] heights = new float[width, length];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < length; y++)
            {
                heights[x, y] = CalculateHeight(x,y);   
            }
        }

        return heights;
    }

    float CalculateHeight(int x, int y)
    {
        float xCor = (float)x / width * scale + randX;
        float yCor = (float)y / length * scale + randY;

        return Mathf.PerlinNoise(xCor, yCor);
    }

    public TerrainData GetTerrainData()
    {
        return terrain.terrainData;
    }
}
