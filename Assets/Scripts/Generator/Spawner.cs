using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner instance;
    [SerializeField] private int noOfAmmoPrefab;
    [SerializeField] private int noOfBears;
    [SerializeField] private int noOfHealtPickupPrefab;
    [SerializeField] private int wandererPresent;
    [SerializeField] private int range;


    public int wandererCount = 0;

    private int groupSize;

    public GameObject ammoPrefab;
    public GameObject bearPrefab;
    public GameObject healthPickupPrefab;
    public GameObject wandererPrefab;
    public GameObject witcherPrefab;
    public GameObject doorPortal;
    private GameObject[] wanderer;

    public GameObject[] figurines;
    public int figurineIndex = 0; 

    public bool isWitcher = false;
    public bool isBear = false;
    public bool isPortal = false;
    public bool isWanderer = false;
    public bool isSecondScene;
    public bool isDeath{ get; set; }

    private Transform player;

    public List<Vector3> treePositions = new List<Vector3>();
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


        if(isWitcher)
            StartCoroutine(WitcherSpawner());
        
        if(isWanderer)
            StartCoroutine(WanderersSpawner());
        
        if (isBear)
            BearSpawner();

        wanderer = GameObject.FindGameObjectsWithTag("Wanderer");

        AmmoSpawner();
        HealthPickupSpawner();
        FigurineHandler();

        figurineIndex = GameData.figurineAmount;
    }

    // Update is called once per frame
    void Update()
    {
        PortalSpawner();
    }


    public void AmmoSpawner()   // this function spawns ammo in the scene
    {
        for(int i = 0; i< noOfAmmoPrefab; i++)
        {
            Vector3 ammoPos;
            bool validPos = false;

            while(!validPos)
            {
               if(isSecondScene) // if ammo pickups are being set to spawn in second scene
               {
                    ammoPos = new Vector3(Random.Range(22, 62), 0f, Random.Range(22, 62));
               }
               else
               {
                    ammoPos = new Vector3(Random.Range(10, 240), 0f, Random.Range(10, 240));
               }

                if (!IsPositionNearTree(ammoPos))
                {
                    validPos = true;
                    GameObject go = Instantiate(ammoPrefab, ammoPos, Quaternion.identity);
                    go.transform.position = new Vector3(go.transform.position.x, 
                        Terrain.activeTerrain.SampleHeight(new Vector3(go.transform.position.x,0f, go.transform.position.z)),
                        go.transform.position.z);
                }
            }
        }
    }


    public void BearSpawner() // this function spawns bear in the scene
    {
        for(int i = 0; i< noOfBears; i++)
        {
            Vector3 bearPos;
            bool validPos = false;

            while(!validPos)
            {
                bearPos = new Vector3(Random.Range(10, 240), 0f, Random.Range(10, 240));

                if(!IsPositionNearTree(bearPos))
                {
                    validPos = true;
                    GameObject go = Instantiate(bearPrefab, bearPos, Quaternion.identity);
                    BearController bC = go.GetComponent<BearController>();
                    
                    // sets the waypoint points and shuffle them for the bear enemy to follow
                    bC.SetWaypoints(waypoints);
                    bC.ShuffleWaypoints();
                }
            }
        }
    }
    
    public void HealthPickupSpawner()   // this function spawns health pickup in the scene
    {
        for(int i = 0; i< noOfHealtPickupPrefab; i++)
        {
            Vector3 healthPickupPos;
            bool validPos = false;

            while(!validPos)
            {
                if(isSecondScene)   // if health pickups are being set to spawn in second scene
                {
                    healthPickupPos = new Vector3(Random.Range(22, 62), 0f, Random.Range(22, 62));
                }
                else
                {
                    healthPickupPos = new Vector3(Random.Range(10, 240), 0f, Random.Range(10, 240));
                }

                if (!IsPositionNearTree(healthPickupPos))
                {
                    validPos = true;
                    GameObject go = Instantiate(healthPickupPrefab, healthPickupPos, Quaternion.identity);
                    go.transform.position = new Vector3(go.transform.position.x, Terrain.activeTerrain.SampleHeight(new Vector3(go.transform.position.x
                        ,0f, go.transform.position.z)), go.transform.position.z);
                }
            }
        }
    }


    
    public IEnumerator WanderersSpawner() // enumerator spawns the wanderer enemies after a random interval in the scene
    {
        yield return new WaitForSeconds(Random.Range(5f, 10f));
        groupSize = Random.Range(2, 6);
        for(int i = 0; i < groupSize; i++)
        {
            GameObject wanderer = objPool.GetPooledObj();

            if (wanderer != null)
            {
                wanderer.GetComponent<EnemyHealthSystem>().currentHealth = 100;
                Vector3 playerPos = GameObject.Find("Tori").transform.position;

                float distanceFromPlayer = 22f;
                float angleFromPlayer = Random.Range(-30f, 30f);

                // set the spawn position infront of the player based on the randomly selected angle
                Vector3 spawnOffset = Quaternion.Euler(0f, angleFromPlayer, 0f) * Vector3.forward * distanceFromPlayer;
                playerPos = new Vector3(Mathf.Clamp(playerPos.x, 10f, 240f), 0f, Mathf.Clamp(playerPos.z, 10f, 240f));

                wanderer.transform.position = playerPos + spawnOffset;
                wanderer.SetActive(true);
            }
        }
        yield return new WaitForSeconds(Random.Range(60f, 90f));
        StartCoroutine(WanderersSpawner());
    }


    public IEnumerator WitcherSpawner() // enumerator for spawning the witcher enemy at random time near the player position
    {
        while (true)
        {
            Vector3 spawnEnemyPos;
            bool validPosition = false;
            yield return new WaitForSeconds(Random.Range(80f, 200f)); 

            while (!validPosition)
            {
                spawnEnemyPos = new Vector3(player.position.x + Random.Range(-5f, 5f), 0f, player.position.z + Random.Range(-5f, 5f));

                if (!IsPositionNearTree(spawnEnemyPos)) // checks for the valid position for the enemy to spawn
                { 
                    validPosition = true;
                    GameObject go = Instantiate(witcherPrefab, spawnEnemyPos, Quaternion.identity);
                    go.transform.position = new Vector3(go.transform.position.x,
                        Terrain.activeTerrain.SampleHeight(new Vector3(go.transform.position.x, 0f, go.transform.position.z)),
                        go.transform.position.z);
                }
            }
            yield return new WaitForSeconds(150f);
            StartCoroutine(WitcherSpawner());
        }
    }

    private bool IsPositionNearTree(Vector3 position) // this function check for possible position around the tree models
    {
        foreach (Vector3 treePos in treePositions)
        {
            if (Vector3.Distance(position, treePos) < 2f) 
                return true;
        }
        return false;
    }
    

    public void PortalSpawner() // spawns the portal door after collecting the figurine piece
    {
        for(int i = 0; i < 1; i++)
        {
            if (figurineIndex == 4 && !isPortal)
            {
                Vector3 pos = new Vector3(220f, 2f, 220f);
                Instantiate(doorPortal, pos, Quaternion.identity);
                isPortal = true;

            }
        }

    }

    public void FigurineHandler() // activates the new figurine piece in the environment 
    {
        figurines[figurineIndex].SetActive(true);
    }
}
