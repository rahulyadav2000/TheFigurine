using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class DieTask : Task
{
    private TaskManager taskManager;
    private NavMeshAgent agent;
    private Animator animator;
    private bool isWanderer;
    public DieTask(TaskManager taskManager, NavMeshAgent navAgent, Animator animator, bool isWanderer) : base(TaskTypes.DIE, taskManager)
    {
        this.agent = navAgent;
        this.taskManager = taskManager;
        this.animator = animator;
        this.isWanderer = isWanderer;
    }

    public override void StartTask()
    {
        agent.ResetPath();
        agent.isStopped = true;
        agent.speed = 0.0f;
        animator.SetBool("walk", false);
        animator.SetBool("run", false);
        animator.SetBool("attack", false);
        animator.SetBool("death", true);
        //TaskManager.Destroy(taskManager.gameObject, 2.5f);
        taskManager.StartCoroutine(DestroyGameObj());
        if (isWanderer)
        {
            taskManager.StartCoroutine(CheckingWandererDead());
        }
        FinishTask();
    }

    public IEnumerator CheckingWandererDead()
    {
        yield return new WaitForSeconds(2.47f);
        taskManager.isWandererDead = true;
    }

    public IEnumerator DestroyGameObj()
    {
        yield return new WaitForSeconds(2.5f);
        taskManager.gameObject.SetActive(false);
    }

}
