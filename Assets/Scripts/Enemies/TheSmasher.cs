using System.Collections;
using System.Collections.Generic;
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
        PatrolTask patrolTask = new PatrolTask(taskManager, navAgent, PatrolPoint(), anim);
        taskManager.AddTask(patrolTask);

        ChaseTask chaseTask = new ChaseTask(taskManager, target, anim, navAgent);
        taskManager.AddTask(chaseTask);

        AttackTask attackTask = new AttackTask(taskManager, anim, navAgent, target);
        taskManager.AddTask(attackTask);

        DieTask dieTask = new DieTask(taskManager, navAgent, anim, isWanderer);
        taskManager.AddTask(dieTask);
    }

    public Vector3 PatrolPoint()
    {
        float xToAdd = Random.Range(-patrolRadius, patrolRadius);
        float zToAdd = Random.Range(-patrolRadius, patrolRadius);
        if (xToAdd >= -2f && xToAdd <= 2f)
            xToAdd *= 5;
        if (zToAdd >= -2f && zToAdd <= 2f)
            zToAdd *= 5;
        Vector3 patrolPoint = startPosition + new Vector3(xToAdd, 0, -(zToAdd));
        return patrolPoint;
    }
}
