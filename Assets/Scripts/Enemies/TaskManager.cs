using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Task;

public class TaskManager : MonoBehaviour
{
    private List<Task> tasks = new List<Task>();
    private Transform player;
    private Task currentTask = null;
    public EnemyHealthSystem healthSystem;
    private Spawner spawner;
    public bool isWandererDead = false;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spawner = Spawner.instance;
    }
    public void AddTask(Task task)
    {
        tasks.Add(task);
    }

    public void RemoveTask()
    {
        tasks.Clear();
    }
    public void LateUpdate()
    {
        tasks.RemoveAll(task => task.isFinished);

        if(tasks.Count == 0) { return; }

        foreach (Task task in tasks)
        {
            if (task.taskTypes == TaskTypes.PATROL)
            {
                task.utilityScore = CallPatrolScore();
            }
            else if (task.taskTypes == TaskTypes.ATTACK)
            {
                task.utilityScore = CallAttackScore();
            }
            else if(task.taskTypes == TaskTypes.ATTACK2)
            {
                task.utilityScore = CallAttack2Score();
            }
            else if(task.taskTypes == TaskTypes.CHASE)
            {
                task.utilityScore = CallChaseScore();
            }
            else if(task.taskTypes == TaskTypes.DIE)
            {
                task.utilityScore = CallDieScore();
            }
        }

        Task selectedTask = GetHighestUtilityTask();

        if (currentTask != null && currentTask != selectedTask)
        {
            currentTask.FinishTask();
            currentTask = null;
        }

        if (currentTask == null && selectedTask != null)
        {
            currentTask = selectedTask;
            PerfomTask(currentTask);
        }
        else if (currentTask != null)
        {
            PerfomTask(currentTask);
        }

        WandererCheck();
    }
    private Task GetHighestUtilityTask()
    {
        float highestScore = float.MinValue;
        Task selectedTask = null;
        foreach (Task task in tasks)
        {
            if (task.utilityScore > highestScore)
            {
                highestScore = task.utilityScore;
                selectedTask = task;
            }
        }
        return selectedTask;
    }

    public void PerfomTask(Task task)
    {
        switch (task.taskTypes)
        {
            case TaskTypes.PATROL:
                PatrolTask patrolTask = (PatrolTask)task;
                patrolTask.StartTask();
                break;
            case TaskTypes.ATTACK:
                AttackTask attackTask = (AttackTask)task;
                attackTask.StartTask();
                break;
            case TaskTypes.CHASE:
                ChaseTask chaseTask = (ChaseTask)task;
                chaseTask.StartTask();
                break;
            case TaskTypes.ATTACK2:
                Attack2Task attack2Task = (Attack2Task)task;
                attack2Task.StartTask();
                break;
            case TaskTypes.DIE:
                DieTask dieTask = (DieTask)task;
                dieTask.StartTask();
                tasks.Remove(task);
                break;
        }
    }

    public float CallPatrolScore()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        float patrolUtility = distance > 7f ? 1.0f : 0.0f;
        //Debug.Log("Patrol Utitliy Score: " + patrolUtility);
        return patrolUtility;
    }

    public float CallAttackScore()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        float attackUtility = distance <= 1.8f ? 1.0f : 0.0f;
        //Debug.Log("Attack Utility Score: " + attackUtility);
        return attackUtility;
    }
    
    public float CallAttack2Score()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        float attack2Utility = distance <= 3.5f && distance > 1.9f ? 1.0f : 0.0f;
        //Debug.Log("Attack Utility Score: " + attackUtility);
        return attack2Utility;
    }

    public float CallChaseScore()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        float chaseUtility = distance < 7f ? 1.0f : 0.0f;
        //Debug.Log("Chase Utility Score: " + chaseUtility);
        return chaseUtility;
    }

    public float CallDieScore()
    {
        float health = healthSystem.GetHealth();
        float dieUtility = health <= 0f ? 1.0f : 0.0f;
        //Debug.Log("Die Utility Score: " + dieUtility);
        return dieUtility;
    }

    public void WandererCheck()
    {
        if(isWandererDead)
        {
            spawner.wandererCount++;
            Debug.Log("Wanderer Dead: " + spawner.wandererCount);
            isWandererDead = false;
            spawner.OnEnemyDeath();
        }
    }
}
