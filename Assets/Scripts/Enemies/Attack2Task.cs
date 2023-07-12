using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.AI;

public class Attack2Task : Task
{
    private NavMeshAgent navAgent;
    private Animator animator;
    private TaskManager taskManager;
    private Transform target;
    private bool isSpecialAttack = false;
    public Attack2Task(TaskManager taskManager, Animator anim, NavMeshAgent agent, Transform target) : base(TaskTypes.ATTACK2, taskManager)
    {
        this.taskManager = taskManager;
        this.animator = anim;
        this.navAgent = agent;
        this.target = target;
    }

    public override void StartTask()
    {
        navAgent.ResetPath();
        navAgent.isStopped= true;
        navAgent.speed= 0.0f;
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
        animator.SetBool("specialAttack", true);
        animator.SetBool("attack", false);
        yield return new WaitForSeconds(3f);

        FinishTask();
    }

}

