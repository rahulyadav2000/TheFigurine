using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Attack2Task : Task
{
    private NavMeshAgent navAgent;
    private Animator animator;
    private TaskManager taskManager;
    private Transform target;
    private bool isSpecialAttack = false;

    private SphereCollider sphereCollider;
    public Attack2Task(TaskManager taskManager, Animator anim, NavMeshAgent agent, Transform target, SphereCollider sphereCollider) : base(TaskTypes.ATTACK2, taskManager)
    {
        this.taskManager = taskManager;
        this.animator = anim;
        this.navAgent = agent;
        this.target = target;
        this.sphereCollider = sphereCollider;
    }

    public override void StartTask() // this class performs the attack2 task
    {
        navAgent.ResetPath();
        navAgent.isStopped= true;
        navAgent.speed= 0.0f;
        navAgent.stoppingDistance = 1.5f;
        animator.SetBool("walk", false);
        animator.SetBool("run", false);
        taskManager.StartCoroutine(StartAttack());
        taskManager.gameObject.transform.LookAt(target);
    }

    IEnumerator StartAttack()
    {
        animator.SetBool("specialAttack", true); 
        animator.SetBool("attack", false);

        yield return new WaitForSeconds(3.2f);
        sphereCollider.enabled = true;
        yield return new WaitForSeconds(0.8f);
        sphereCollider.enabled= false;

        FinishTask();
        yield return new WaitForSeconds(1.5f);
    }
}

