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

        // generate list of tree position through Generate Points 
        List<Vector3> treePos = GeneratePoints(size, minDist, maxTrees, maxAttempts);
        TerrainData terrainData = Terrain.activeTerrain.terrainData;

        foreach (Vector3 originalPosition in treePos)
        {
            // adjusts the tree position to the height of the terrain
            Vector3 treePosition = new Vector3(originalPosition.x, 
                Terrain.activeTerrain.SampleHeight(new Vector3(originalPosition.x, 0f, originalPosition.z)), 
                originalPosition.z);

            Instantiate(treePrefab, treePosition, Quaternion.identity);
        }
    }

    // generate points for placing of trees through Poisson Disc Sampling
    private List<Vector3> GeneratePoints(Vector3 regionSize, float minDist, int maxSamples, int maxAttempts)
    {
        List<Vector3> samples = new List<Vector3>();
        List<Vector3> nextPoints = new List<Vector3>();

        float cellSize = minDist / Mathf.Sqrt(2);   // calculates the size of grid cell 

        // grid initialisation to mark the placed points
        int[,] grid = new int[Mathf.CeilToInt(regionSize.x / cellSize), Mathf.CeilToInt(regionSize.z / cellSize)];

        // marks centre point for the first point
        Vector2 spawnPoint = regionSize / 2;
        nextPoints.Add(spawnPoint);

        samples.Add(spawnPoint);

        // marks the first point in the grid
        grid[(int)(spawnPoint.x / cellSize), (int)(spawnPoint.y / cellSize)] = samples.Count;

        while(nextPoints.Count > 0 && samples.Count < maxSamples) {
        
            // randomly selects a point
            int spawnIndex = Random.Range(0, nextPoints.Count);
            Vector3 spawnCentre = nextPoints[spawnIndex];

            bool acceptedPoint = false;

            for (int i = 0; i < maxAttempts; i++){
                float angle = Random.value * Mathf.PI * 2; // generate the random angle in any direction

                // calculate position of new point based on the angle and direction
                Vector3 dir = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));
                Vector3 individualPoint = spawnCentre + dir * Random.Range(minDist, 2 * minDist);

                // checks for the acceptable position for  the point
                if (AcceptablePos(individualPoint, regionSize, cellSize, minDist, samples, grid))
                {
                    samples.Add(individualPoint);
                    nextPoints.Add(individualPoint);
                    grid[(int)(individualPoint.x / cellSize), (int)(individualPoint.z / cellSize)] = samples.Count;
                    acceptedPoint = true;
                    break;
                }
            }
            if (!acceptedPoint){
                nextPoints.RemoveAt(spawnIndex); // removes the current spawn point from the list 
            }
        }
        return samples; // returns all the generated samples 
    }

    private bool AcceptablePos(Vector3 individualPoint, Vector3 regionSize, float cellSize, float minDis, List<Vector3> samples, int[,] grid){
        // checks for the point is within the region 
        if(individualPoint.x > 0 && individualPoint.x < regionSize.x && individualPoint.z > 0 && individualPoint.z < regionSize.z){

            // calculates the indices of individual point
            int blockX = (int)(individualPoint.x / cellSize);
            int blockZ = (int)(individualPoint.z / cellSize);

            // checks all the samples are within the region boundary
            int originX = Mathf.Max(0, blockX - 2);
            int endX = Mathf.Min(blockX + 2, grid.GetLength(0) - 1);
            int originZ = Mathf.Max(0, blockZ - 2);
            int endZ = Mathf.Min(blockZ + 2, grid.GetLength(1) - 1);

            // iterate through the nearby grid cells to check the possiblity of the point
            for (int x = originX; x <= endX; x++)
            {
                for (int z = originZ; z <= endZ; z++)
                {
                    int sampleIndex = grid[x, z] - 1;

                    // if sample is present at this grid cell
                    if (sampleIndex != -1)
                    {
                        // calculates the distance between the individual point and existing points
                        float distance = Vector3.Distance(individualPoint, samples[sampleIndex]);

                        // checks if the distance of the point is less than minimun distance and returns false 
                        if (distance < minDis)
                        {
                            return false;
                        }
                    }
                }
            }
            return true; // returns true for the acceptable position
        }
        return false;
    }
}
