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

    public override void StartTask() // this class performs the die task for the enemy wanderer or smasher
    {
        agent.ResetPath();
        agent.isStopped = true;
        agent.speed = 0.0f;
        animator.SetBool("walk", false);
        animator.SetBool("run", false);
        animator.SetBool("attack", false);
        animator.ResetTrigger("attack");
        animator.SetBool("death", true);
        taskManager.StartCoroutine(DestroyGameObj());
        if (isWanderer)
        {
            taskManager.StartCoroutine(CheckingWandererDead());
        }
        FinishTask();
    }

    public IEnumerator CheckingWandererDead() // keeps track of the wanderer enemy death
    {
        yield return new WaitForSeconds(2.47f);
        taskManager.isWandererDead = true;
    }

    public IEnumerator DestroyGameObj()
    {
        yield return new WaitForSeconds(2.5f);
        taskManager.gameObject.SetActive(false); // sets the enemy gameobjet to false for the object pool
    }

}
