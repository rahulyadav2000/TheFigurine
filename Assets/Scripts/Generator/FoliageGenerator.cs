using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoliageGenerator : MonoBehaviour
{
    public GameObject treePrefab;
    public int maxAttempts = 20;
    public float minDist = 10f;
    public int maxTrees = 40;
    [SerializeField] private float terrainSize = 150;

    public Terrain terrain;

    // Start is called before the first frame update
    void Start()
    {
        GenerateTrees();
    }
    private void GenerateTrees()
    {
        Vector3 size = new Vector3(terrainSize , 0, terrainSize);
        List<Vector3> treePos = GeneratePoints(size, minDist, maxTrees, maxAttempts);
        TerrainData terrainData = Terrain.activeTerrain.terrainData;

        foreach (Vector3 originalPosition in treePos)
        {
            Vector3 treePosition = new Vector3(originalPosition.x, 
                Terrain.activeTerrain.SampleHeight(new Vector3(originalPosition.x, 0f, originalPosition.z)), 
                originalPosition.z);

            Instantiate(treePrefab, treePosition, Quaternion.identity);
        }
    }

        private List<Vector3> GeneratePoints(Vector3 regionSize, float minDist, int maxSamples, int maxAttempts)
    {
        List<Vector3> samples = new List<Vector3>();
        List<Vector3> nextPoints = new List<Vector3>();

        float cellSize = minDist / Mathf.Sqrt(2);
        int[,] grid = new int[Mathf.CeilToInt(regionSize.x / cellSize), Mathf.CeilToInt(regionSize.z / cellSize)];

        Vector2 spawnPoint = regionSize / 2;
        nextPoints.Add(spawnPoint);

        samples.Add(spawnPoint);

        grid[Mathf.FloorToInt(spawnPoint.x / cellSize), Mathf.FloorToInt(spawnPoint.y / cellSize)] = samples.Count;

        while(nextPoints.Count > 0 && samples.Count < maxSamples) {
        
            int spawnIndex = Random.Range(0, nextPoints.Count);
            Vector3 spawnCentre = nextPoints[spawnIndex];

            bool acceptedPoint = false;

            for (int i = 0; i < maxAttempts; i++){
                float angle = Random.value * Mathf.PI * 2;

                Vector3 dir = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));
                Vector3 individualPoint = spawnCentre + dir * Random.Range(minDist, 2 * minDist);

                if (AcceptablePos(individualPoint, regionSize, cellSize, minDist, samples, grid))
                {
                    samples.Add(individualPoint);
                    nextPoints.Add(individualPoint);
                    grid[Mathf.FloorToInt(individualPoint.x / cellSize), Mathf.FloorToInt(individualPoint.z / cellSize)] = samples.Count;
                    acceptedPoint = true;
                    break;
                }
            }
            if (!acceptedPoint){
                nextPoints.RemoveAt(spawnIndex);
            }
        }
        return samples;
    }

    private bool AcceptablePos(Vector3 individualPoint, Vector3 regionSize, float cellSize, float minDis, List<Vector3> samples, int[,] grid){
        
        if(individualPoint.x > 0 && individualPoint.x < regionSize.x && individualPoint.z > 0 && individualPoint.z < regionSize.z){
            int blockX = Mathf.FloorToInt(individualPoint.x / cellSize);
            int blockZ = Mathf.FloorToInt(individualPoint.z / cellSize);
            int originX = Mathf.Max(0, blockX - 2);
            int endX = Mathf.Min(blockX + 2, grid.GetLength(0) - 1);
            int originZ = Mathf.Max(0, blockZ - 2);
            int endZ = Mathf.Min(blockZ + 2, grid.GetLength(1) - 1);
            for (int x = originX; x <= endX; x++)
            {
                for (int z = originZ; z <= endZ; z++)
                {
                    int sampleIndex = grid[x, z] - 1;
                    if (sampleIndex != -1)
                    {
                        float sqrDistance = (individualPoint - samples[sampleIndex]).sqrMagnitude;
                        if (sqrDistance < minDis* minDis)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }
        
        return false;
    }
}
