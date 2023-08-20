using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MultiAttackTask : Task
{
    private TaskManager taskManager;
    private Animator animator;
    private NavMeshAgent navAgent;
    private Transform target;
    public MultiAttackTask(TaskManager taskManager, Animator anim, NavMeshAgent agent, Transform target) : base(TaskTypes.MULTIATTACK, taskManager)
    {
        this.taskManager = taskManager;
        this.animator = anim;
        this.navAgent = agent;
        this.target = target;
    }

    public override void StartTask() // this class performs the random attack animations for the enemy smasher
    {
        navAgent.ResetPath();
        navAgent.isStopped = true;
        navAgent.speed = 0.0f;
        navAgent.stoppingDistance = 1.5f;
        animator.SetBool("walk", false);
        animator.SetBool("run", false);
        //Debug.Log("working!!");
        taskManager.StartCoroutine(StartAttack());
        taskManager.gameObject.transform.LookAt(target);
    }

    IEnumerator StartAttack()
    {
        yield return new WaitForSeconds(1);
        animator.SetInteger("index", Random.Range(0, 3));
        animator.SetTrigger("attack");
        FinishTask();
    }
}
