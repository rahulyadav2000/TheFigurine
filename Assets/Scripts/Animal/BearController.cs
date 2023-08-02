using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BearController : MonoBehaviour
{
    [SerializeField] private List<Transform> waypointsList = new List<Transform>();
    private Transform targetWaypoint;
    private int index;
    private float minDistanceToWaypoint = 2.1f;
    private int lastWaypointIndex;

    public NavMeshAgent agent;
    public Animator animator;
    public EnemyHealthSystem bearHealth;

    [SerializeField] private float range;
    
    private Transform playerTransform;

    public GameObject meatPrefab;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(waypointsList != null)
        {
            Vector3 directionToWaypoint = (targetWaypoint.position - transform.position).normalized;

            Quaternion rotationForWaypoint = Quaternion.LookRotation(directionToWaypoint);

            transform.rotation = Quaternion.Slerp(transform.rotation, rotationForWaypoint, 2f * Time.deltaTime);

            float dist = Vector3.Distance(transform.position, targetWaypoint.position);
            CheckNextWaypoint(dist);

            agent.speed = 0.7f;
            agent.SetDestination(targetWaypoint.position);

            animator.SetBool("walk", true);
            animator.SetBool("run", false);
        }
        ChasePlayerTask();

        if(bearHealth.GetHealth() <= 0) 
        {
            agent.speed = 0.0f;
            animator.SetBool("death", true);
            Destroy(gameObject, 3.6f);
            animator.ResetTrigger("attack");
            animator.SetBool("run", false);
            animator.SetBool("walk", false);
            Invoke(nameof(MeatSpawner), 3.58f);
        }
    }

    public void MeatSpawner()
    {
        if (meatPrefab != null)
        {
            Vector3 meatPos = new Vector3(transform.position.x + 1f, 
                Terrain.activeTerrain.SampleHeight(new Vector3(transform.position.x, 0f, transform.position.z)) + .7f, transform.position.z + 1f);
            GameObject go = Instantiate(meatPrefab, meatPos, Quaternion.identity);
        }
    }

    public void CheckNextWaypoint(float currentDistance)
    {
        if(currentDistance < minDistanceToWaypoint)
        {
            index++;
            CallForLastWaypoint();
        }
    }

    public void CallForLastWaypoint()
    {
        if(index > lastWaypointIndex)
        {
            index = 0;
        }
        targetWaypoint = waypointsList[index];
    }

    public void ChasePlayerTask()
    {
        float distance = Vector3.Distance(transform.position, playerTransform.position);

        if(distance < range)
        {
            animator.SetBool("walk", false);
            agent.speed = 1.5f;
            agent.SetDestination(playerTransform.position);
            animator.SetBool("run", true);

            transform.LookAt(playerTransform.position);
        }

        if (distance <= 1.3f)
        {
            animator.SetInteger("index", Random.Range(0, 4));
            animator.SetTrigger("attack");

            animator.SetBool("run", false);
            animator.SetBool("walk", false);
            transform.LookAt(playerTransform.position);
        }
    }

    public void SetWaypoints(List<Transform> waypoints)
    {
        waypointsList = waypoints;
        if (waypointsList.Count > 0)
        {
            lastWaypointIndex = waypointsList.Count - 1;
            targetWaypoint = waypointsList[index];
        }
    }

    public void ShuffleWaypoints()
    {
        List<Transform> randomWP = new List<Transform>(waypointsList);

        int index = randomWP.Count;

        while (index > 1)
        {
            index--;
            int x = Random.Range(0, index + 1);

            Transform temp = randomWP[x];
            randomWP[x] = randomWP[index];
            randomWP[index] = temp;
        }

        waypointsList = randomWP;
    }

}
