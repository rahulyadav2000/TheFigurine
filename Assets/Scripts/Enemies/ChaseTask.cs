using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseTask : Task
{
    private NavMeshAgent agent;
    private Transform destination;
    private Animator animator;
    private TaskManager taskManager;
    public ChaseTask(TaskManager taskManager, Transform destination, Animator animator, NavMeshAgent navAgent) : base(TaskTypes.CHASE, taskManager)
    {
        this.destination = destination;
        this.animator = animator;
        this.agent = navAgent;
        this.taskManager = taskManager;
    }

    public override void StartTask() // this class performs the chase task to chase the player
    {
        animator.SetBool("walk", false);
        animator.SetBool("attack", false);
        agent.isStopped= false;
        agent.speed = 2f;
        animator.SetBool("run", true);
        agent.SetDestination(destination.position);
        taskManager.gameObject.transform.LookAt(destination);
        Vector3 direc = (taskManager.gameObject.transform.position - destination.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(-(new Vector3(direc.x, 0.0f, direc.z)));
        taskManager.gameObject.transform.rotation = Quaternion.Slerp(taskManager.gameObject.transform.rotation, targetRotation, 1);
        FinishTask();
    }
}
