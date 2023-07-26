using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner instance;
    [SerializeField] private int noOfSmashers = 4;
    [SerializeField] private int wandererPresent;
    [SerializeField] private int range = 75;
    
    public int wandererCount = 0;
    private int enemyCount = 0;
    [SerializeField] private int groupSize = 2;

    public GameObject smasherPrefab;
    public GameObject wandererPrefab;
    public GameObject witcherPrefab;
    public GameObject chestPrefab;
    public GameObject enemyPrefab;
    public GameObject doorPortal;
    private GameObject[] wanderer;

    public bool isWanderer = false;
    public bool isWitcher = false;
    public bool isSmasher = false;
    public bool isDeath{ get; set; }

    private Transform player;

    public List<Vector3> treePositions = new List<Vector3>();
    private List<Vector3> smasherPositions = new List<Vector3>();


    public ObjectPool objPool;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        objPool = ObjectPool.instance;
        isDeath = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if(isSmasher)
            SmasherSpawner();
            ChestSpawner();

        if(isWanderer)
            WandererSpawner();

        if(isWitcher)
            StartCoroutine(WitcherSpawner());

        wanderer = GameObject.FindGameObjectsWithTag("Wanderer");
    }

    // Update is called once per frame
    void Update()
    {
        PortalSpawner();
    }

    public void ChestSpawner()
    {
        GameObject[] smashers = GameObject.FindGameObjectsWithTag("Smasher");

        for (int i = 0; i < smashers.Length; i++)
        {
            Vector3 spawnPos = smashers[i].transform.position + new Vector3(5f, 0f, 5f);
            Instantiate(chestPrefab, spawnPos, Quaternion.identity);
        }
    }

    public void SmasherSpawner()
    {
        for (int i = 0; i < noOfSmashers; i++)
        {
            Vector3 spawnEnemyPos;
            bool validPosition = false;

            while (!validPosition)
            {
                spawnEnemyPos = new Vector3(Random.Range(0, range), 0f, Random.Range(0, range));

                if (!IsPositionNearTree(spawnEnemyPos) && !IsPositionNearSmasher(spawnEnemyPos))
                {
                    validPosition = true;
                    Instantiate(smasherPrefab, spawnEnemyPos, Quaternion.identity);
                }
            }
        }
    }

    public void WandererSpawner()
    {
        for(int i = 0; i < groupSize; i++)
        {
            GameObject wanderer = objPool.GetPooledObj();
            if (wanderer == null) return;

            if(wanderer != null)
            {
                Vector3 playerPos = GameObject.Find("Tori").transform.position;
                /*playerPos += Random.insideUnitSphere.normalized * 35;
                playerPos.y = 0f;
                playerPos = new Vector3(Mathf.Clamp(playerPos.x, 0f, 150f), 0f, Mathf.Clamp(playerPos.z, 0f, 150f));*/

                float distanceFromPlayer = 20f;
                float angleFromPlayer = Random.Range(0f, 180f);

                Vector3 spawnOffset = Quaternion.Euler(0f, angleFromPlayer, 0f) * Vector3.forward * distanceFromPlayer;
                playerPos = new Vector3(Mathf.Clamp(playerPos.x, 0f, 150f), 0f, Mathf.Clamp(playerPos.z, 0f, 150f));

                wanderer.transform.position = playerPos + spawnOffset;
                wanderer.SetActive(true);
                enemyPrefab.SetActive(false);
                enemyCount++;
            }
        }
    }
    
    public void OnEnemyDeath()
    {
        enemyCount--;
        if(enemyCount <= 0)
        {
            //WandererSpawner();
        }
    }

    public IEnumerator WitcherSpawner()
    {
        while (true)
        {
            Vector3 spawnEnemyPos;
            bool validPosition = false;
            yield return new WaitForSeconds(Random.Range(55f, 120f)); 

            while (!validPosition)
            {
                spawnEnemyPos = new Vector3(player.position.x + Random.Range(-5f, 5f), 0f, player.position.z + Random.Range(-5f, 5f));

                if (!IsPositionNearTree(spawnEnemyPos))
                { 
                    validPosition = true;
                    Instantiate(witcherPrefab, spawnEnemyPos, Quaternion.identity);
                }
            }
            yield return new WaitForSeconds(125f);
            StartCoroutine(WitcherSpawner());
        }
    }

    private bool IsPositionNearTree(Vector3 position)
    {
        foreach (Vector3 treePos in treePositions)
        {
            if (Vector3.Distance(position, treePos) < 2f) 
                return true;
        }
        return false;
    }
    
    private bool IsPositionNearSmasher(Vector3 position)
    {
        foreach (Vector3 smasherPos in smasherPositions)
        {
            if (Vector3.Distance(position, smasherPos) < 50f) 
                return true;
        }
        return false;
    }

    public void PortalSpawner()
    {
        if(wandererCount == 5)
        {
            Vector3 pos = new Vector3(145, 0, 145);
            Instantiate(doorPortal, pos, Quaternion.identity);
            wandererCount = 0;

            WandererSpawner();
        }
    }
}
