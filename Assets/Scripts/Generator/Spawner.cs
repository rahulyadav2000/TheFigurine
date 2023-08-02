using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner instance;
    [SerializeField] private int noOfSmashers = 4;
    [SerializeField] private int noOfAmmoPrefab;
    [SerializeField] private int noOfBears;
    [SerializeField] private int noOfHealtPickupPrefab;
    [SerializeField] private int wandererPresent;
    [SerializeField] private int range;

    public LayerMask groundLayer;

    public int wandererCount = 0;

    [SerializeField] private int groupSize = 2;

    public GameObject ammoPrefab;
    public GameObject bearPrefab;
    public GameObject healthPickupPrefab;
    public GameObject smasherPrefab;
    public GameObject wandererPrefab;
    public GameObject witcherPrefab;
    public GameObject chestPrefab;
    public GameObject doorPortal;
    private GameObject[] wanderer;

    public bool isWitcher = false;
    public bool isSmasher = false;
    public bool isBear = false;
    public bool isDeath{ get; set; }

    private Transform player;

    public List<Vector3> treePositions = new List<Vector3>();
    private List<Vector3> smasherPositions = new List<Vector3>();
    public List<Transform> waypoints = new List<Transform>();


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


        if(isWitcher)
            StartCoroutine(WitcherSpawner());


        if (isBear)
            BearSpawner();

        wanderer = GameObject.FindGameObjectsWithTag("Wanderer");

        AmmoSpawner();
        HealthPickupSpawner();
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

    public void AmmoSpawner()
    {
        for(int i = 0; i< noOfAmmoPrefab; i++)
        {
            Vector3 ammoPos;
            bool validPos = false;

            while(!validPos)
            {
               
                ammoPos = new Vector3(Random.Range(10, 85), 0f, Random.Range(10, 120));

                if(!IsPositionNearTree(ammoPos))
                {
                    validPos = true;
                    GameObject go = Instantiate(ammoPrefab, ammoPos, Quaternion.identity);
                    go.transform.position = new Vector3(go.transform.position.x, Terrain.activeTerrain.SampleHeight(new Vector3(go.transform.position.x
                        , 0f, go.transform.position.z)) , go.transform.position.z);
                }
            }
        }
    }


    public void BearSpawner()
    {
        for(int i = 0; i< noOfBears; i++)
        {
            Vector3 bearPos;
            bool validPos = false;

            while(!validPos)
            {
                bearPos = new Vector3(Random.Range(10, 85), 0f, Random.Range(10, 120));

                if(!IsPositionNearTree(bearPos))
                {
                    validPos = true;
                    GameObject go = Instantiate(bearPrefab, bearPos, Quaternion.identity);
                    BearController bC = go.GetComponent<BearController>();
                    bC.SetWaypoints(waypoints);
                    bC.ShuffleWaypoints();
                }
            }
        }
    }
    
    public void HealthPickupSpawner()
    {
        for(int i = 0; i< noOfHealtPickupPrefab; i++)
        {
            Vector3 healthPickupPos;
            bool validPos = false;

            while(!validPos)
            {
                healthPickupPos = new Vector3(Random.Range(10, 85), 0f, Random.Range(10, 120));

                if(!IsPositionNearTree(healthPickupPos))
                {
                    validPos = true;
                    GameObject go = Instantiate(healthPickupPrefab, healthPickupPos, Quaternion.identity);
                    go.transform.position = new Vector3(go.transform.position.x, Terrain.activeTerrain.SampleHeight(new Vector3(go.transform.position.x
                        ,0f, go.transform.position.z)), go.transform.position.z);
                }
            }
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
                spawnEnemyPos = new Vector3(Random.Range(0, range - 60), 0f, Random.Range(0, range));

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
            wanderer.GetComponent<EnemyHealthSystem>().currentHealth = 100;
            if (wanderer == null) return;

            if(wanderer != null)
            {
                Vector3 playerPos = GameObject.Find("Tori").transform.position;
                

                float distanceFromPlayer = 25f;
                float angleFromPlayer = Random.Range(-60f, 60f);

                Vector3 spawnOffset = Quaternion.Euler(0f, angleFromPlayer, 0f) * Vector3.forward * distanceFromPlayer;
                playerPos = new Vector3(Mathf.Clamp(playerPos.x, 0f, 70f), 0f, Mathf.Clamp(playerPos.z, 0f, 130f));

                wanderer.transform.position = playerPos + spawnOffset;
                wanderer.SetActive(true);
            }
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
            Vector3 pos = new Vector3(85, 0, 145);
            Instantiate(doorPortal, pos, Quaternion.identity);
            wandererCount = 0;
        }
    }
}
