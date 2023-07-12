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
    public AttackTask(TaskManager taskManager, Animator anim, NavMeshAgent agent, Transform target) : base(TaskTypes.ATTACK, taskManager)
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
        animator.SetBool("attack", true);
        animator.SetBool("specialAttack", false);
        //taskManager.StartCoroutine(SmasherAttack());

/*        yield return new WaitForSeconds(Random.Range(2f, 10f));

        if (!isSpecialAttack)
        {
            animator.SetBool("specialAttack", true);
            animator.SetBool("attack", false);

            isSpecialAttack = true;
        }


        animator.SetBool("specialAttack", false);
        yield return new WaitForSeconds(3f);

        animator.SetBool("attack", true);*/

        FinishTask();

        
    }

/*    public IEnumerator SmasherAttack()
    {
        float dis = Vector3.Distance(taskManager.gameObject.transform.position, target.position);
        if (dis <= 3f && dis > 1.8f && isSmasher)
        {
            Debug.Log("Working!!");
            animator.SetBool("attack", false);
            animator.SetBool("roar", true);
            yield return new WaitForSeconds(5.3f);
            animator.SetBool("roar", false);
            animator.SetBool("smashAttack", true);

        }
        else if (dis < 1.8f && isSmasher)
        {
            animator.SetBool("smashAttack", false);
            animator.SetBool("attack", true);
        }
        else
        {
            animator.SetBool("attack", true);

        }
    }*/

}

