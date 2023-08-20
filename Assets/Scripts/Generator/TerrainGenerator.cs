using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
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

    public NavMeshSurface navMeshSurface;

    private void Start()
    {
        NavMeshBuilder();
        // gets a random value for randX and randY
        randX = Random.Range(0, width);
        randY = Random.Range(0, length);
    }
    private void Update()
    {
        terrain.terrainData = GenerateTerrain(terrain.terrainData);
    }

    TerrainData GenerateTerrain(TerrainData terrainData)
    {
        // set the height map resolution to the width +1
        terrainData.heightmapResolution = width + 1;

        // set the size of the terrain 
        terrainData.size = new Vector3(width, height, length);
        
        terrainData.SetHeights(0, 0, GenerateHeights()); // sets the randomly generated heights for the terrain

        return terrainData;
    }

    float[,] GenerateHeights()
    {
        float[,] heights = new float[width, length];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < length; y++)
            {
/*                float xCor = (float)x / width * scale + randX;
                float yCor = (float)y / length * scale + randY;
*/
                float xCor = (float)x / width * scale;
                float yCor = (float)y / length * scale;
                heights[x, y] = Mathf.PerlinNoise(xCor, yCor);
            }
        }
        return heights; // returns the random height for the terrain
    }

    public void NavMeshBuilder()
    {
        navMeshSurface.BuildNavMesh(); // bakes the terrain for the nav mesh agents to function at runtime
    }
}
