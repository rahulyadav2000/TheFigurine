using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.AI;

public class AttackTask : Task
{
    private NavMeshAgent navAgent;
    private Animator animator;
    private TaskManager taskManager;
    private Transform target;
    private bool isSpecialAttack = false;

    private SphereCollider armCollider;
    public AttackTask(TaskManager taskManager, Animator anim, NavMeshAgent agent, Transform target, SphereCollider armCollider) : base(TaskTypes.ATTACK, taskManager)
    {
        this.taskManager = taskManager;
        this.animator = anim;
        this.navAgent = agent;
        this.target = target;
        this.armCollider = armCollider;
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
        animator.SetBool("attack", true);
        animator.SetBool("specialAttack", false);
        armCollider.enabled = true;
        yield return new WaitForSeconds(1);
        armCollider.enabled = false;

        FinishTask();

        
    }

}

