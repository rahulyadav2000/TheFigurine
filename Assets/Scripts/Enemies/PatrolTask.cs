using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolTask : Task
{
    private Animator anim;
    private NavMeshAgent navAgent;
    private Vector3 patrolPoint;
    private float idleTime;
    private TaskManager taskManager;
   public PatrolTask(TaskManager taskManager, NavMeshAgent agent, Vector3 patrolPoints, Animator animator) : base(TaskTypes.PATROL, taskManager)
    {
        this.navAgent = agent;
        this.patrolPoint = patrolPoints;
        this.anim = animator;
        this.taskManager = taskManager;
        idleTime = Random.Range(0.5f, 2f);
    }

    public override void StartTask()
    {
        anim.SetBool("attack", false);
        anim.SetBool("run", false);
        navAgent.isStopped = false;
        navAgent.speed = 1;
        anim.SetBool("walk", true);
        taskManager.StartCoroutine(StartPatrol());
    }

    public IEnumerator StartPatrol() // assings the patrol points for the enemy and set their destination
    {        
        yield return new WaitForSeconds(idleTime);

        Vector3 direction = (patrolPoint - navAgent.gameObject.transform.position).normalized;

        if(direction !=Vector3.zero)
        {
            Quaternion directionalRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            navAgent.gameObject.transform.rotation = Quaternion.Lerp(navAgent.gameObject.transform.rotation, directionalRotation, 1);
        }

        navAgent.SetDestination(patrolPoint);

        FinishTask();
    }
}
