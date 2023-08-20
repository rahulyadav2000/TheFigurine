using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class TheSmasher : MonoBehaviour
{
    [SerializeField] private float patrolRadius = 6f;


    private Vector3 startPosition;
    public Animator anim;
    public TaskManager taskManager;
    public NavMeshAgent navAgent;
    private Transform target;
    private bool isWanderer = false;
    public void Start()
    {
        startPosition = transform.position;

        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        //adds the task for smasher enemy in the tasks list 

        PatrolTask patrolTask = new PatrolTask(taskManager, navAgent, PatrolPoint(), anim);
        taskManager.AddTask(patrolTask);

        ChaseTask chaseTask = new ChaseTask(taskManager, target, anim, navAgent);
        taskManager.AddTask(chaseTask);

        MultiAttackTask multiAttackTask = new MultiAttackTask(taskManager, anim, navAgent, target);
        taskManager.AddTask(multiAttackTask);

        DieTask dieTask = new DieTask(taskManager, navAgent, anim, isWanderer);
        taskManager.AddTask(dieTask);
    }

    public Vector3 PatrolPoint() // this function calculates and return a random patrol point for the smasher enemy within a patrol radius
    {
        float xCoord = Random.Range(-patrolRadius, patrolRadius);
        float zCoord = Random.Range(-patrolRadius, patrolRadius);
        if (xCoord >= -2f && xCoord <= 2f)
            xCoord *= 5; // multiply xCoord with 5 to encourage the movement when xCoord is too small 
        if (zCoord >= -2f && zCoord <= 2f)
            zCoord *= 5;    // multiply zCoord with 5 to encourage the movement when zCoord is too small

        // calculates the patrolPoint by adding a new xCoord and zCoord value to the start position of the smasher enemy
        Vector3 patrolPoint = startPosition + new Vector3(xCoord, 0, -(zCoord));
        return patrolPoint;
    }
}
