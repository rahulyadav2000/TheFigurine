using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task 
{
    public enum TaskTypes // enum class for task type
    {
        PATROL,
        ATTACK,
        ATTACK2,
        MULTIATTACK,
        CHASE,
        DIE
    }
    public TaskTypes taskTypes;
    public float utilityScore;
    public TaskManager tM;
    public bool isFinished { get; protected set; } // boolean for the checking the task status. whether the task is finished or not
    public Task(TaskTypes type, TaskManager taskManager)
    {
        this.taskTypes= type;
        this.tM = taskManager;
    }

    public virtual void StartTask() { } // virtual function which performs the logic for every task

    public void FinishTask()
    {
        isFinished= true;
    }
}
