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
    public void AddTask(Task task) // adds the task into the tasks list
    {
        tasks.Add(task);
    }

    public void RemoveTask() //  removes the task from the tasks list
    {
        tasks.Clear();
    }
    public void LateUpdate()
    {
        tasks.RemoveAll(task => task.isFinished);   // emptys the list before assigning the task

        if(tasks.Count == 0) { return; }

        foreach (Task task in tasks)
        {
            UpdateTaskUtilityScore(task);
        }

        Task selectedTask = GetHighestUtilityTask();

        // checks if there is any processed task and the task is not the selected task
        if (currentTask != null && currentTask != selectedTask)
        {
            currentTask.FinishTask(); // finishes the task
            currentTask = null;
        }

        // checks if there is no task and there is a task with the highest utility score
        if (currentTask == null && selectedTask != null)
        {
            currentTask = selectedTask; // assigns current task to the task with highest utility score
            PerfomTask(currentTask); // performs the current task
        }
        // check if there is a task and performs the current task
        else if (currentTask != null)
        {
            PerfomTask(currentTask);
        }

        WandererCheck();
    }

    public void UpdateTaskUtilityScore(Task task) // the funstion updates the utility score based on task type
    {
        switch (task.taskTypes)
        {
            case TaskTypes.PATROL:
                PatrolTask patrolTask = (PatrolTask)task;
                task.utilityScore = CallPatrolScore(); // calculates the utility score for the patrol task 
                break;

            case TaskTypes.ATTACK:
                AttackTask attackTask = (AttackTask)task;
                task.utilityScore = CallAttackScore();  // calculates the utility score for the attack task
                break;

            case TaskTypes.MULTIATTACK:
                MultiAttackTask multiAttackTask = (MultiAttackTask)task;
                task.utilityScore = CallMultiAttackScore(); // calculates the utility score for the multiattack task
                break;

            case TaskTypes.ATTACK2:
                Attack2Task attack2Task = (Attack2Task)task;
                task.utilityScore = CallAttack2Score(); // calculates the utility score for the attack2 task
                break;

            case TaskTypes.CHASE:
                ChaseTask chaseTask = (ChaseTask)task;
                task.utilityScore = CallChaseScore();   // calculates the utility score for the chase task
                break;

            case TaskTypes.DIE:
                DieTask dieTask = (DieTask)task;
                task.utilityScore = CallDieScore(); // calculates the utility score for the die task
                break;
        }
    }

    private Task GetHighestUtilityTask() //this function selects and returns the task with highest score from the task list
    {
        float highestScore = float.MinValue;
        Task selectedTask = null;
        foreach (Task task in tasks)
        {
            if (task.utilityScore > highestScore)
            {
                highestScore = task.utilityScore; // assigns the highest score to the utility score of the current task
                selectedTask = task; // assigns the selected task with the highest score task
            }
        }
        return selectedTask;
    }

    public void PerfomTask(Task task)
    {
        task.StartTask(); // calls the starttask function to start task
    }

    public float CallPatrolScore()
    {
        float distance = Vector3.Distance(transform.position, player.position); 
        float patrolUtility = distance > 9f ? 1.0f : 0.0f;
        return patrolUtility;   // scores the patrol task based on the distance between the player and enemy
    }

    public float CallAttackScore()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        float attackUtility = distance <= 1.5f ? 1.0f : 0.0f;
        return attackUtility;   // scores the attack task based on the distance between the player and enemy
    }
    
    public float CallAttack2Score()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        float attack2Utility = distance <= 2.8f && distance > 1.8f ? 1.0f : 0.0f;
        return attack2Utility;  // scores the attack task based on the distance between the player and enemy
    }
    
    public float CallMultiAttackScore()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        float multiAttackUtility = distance <= 2.3f && distance > 1f ? 1.0f : 0.0f;
        return multiAttackUtility;  // scores the attack task based on the distance between the player and enemy
    }

    public float CallChaseScore()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        float chaseUtility = distance < 9f ? 1.0f : 0.0f;
        return chaseUtility;    // scores the chase task based on the distance between the player and enemy
    }

    public float CallDieScore()
    {
        float health = healthSystem.GetHealth();
        float dieUtility = health <= 0f ? 1.0f : 0.0f;
        return dieUtility;  // scores the die task based on the current health of the enemy
    }

    public void WandererCheck() // tells about the counter of the wanderer enemy death
    {
        if(isWandererDead)
        {
            spawner.wandererCount++;
            isWandererDead = false;
        }
    }
}
