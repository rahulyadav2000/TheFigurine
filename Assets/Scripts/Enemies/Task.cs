using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task 
{
    public enum TaskTypes
    {
        PATROL,
        ATTACK,
        ATTACK2,
        CHASE,
        DIE
    }
    public TaskTypes taskTypes;
    public float utilityScore;
    public TaskManager tM;
    public bool isFinished { get; protected set; }
    public Task(TaskTypes type, TaskManager taskManager)
    {
        this.taskTypes= type;
        //this.utilityScore = score;
        this.tM = taskManager;
    }

    public virtual void StartTask() { }

    public void FinishTask()
    {
        isFinished= true;
    }
}
