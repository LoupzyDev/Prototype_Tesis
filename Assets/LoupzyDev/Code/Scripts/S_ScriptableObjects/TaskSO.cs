using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TasksData", menuName = "ScriptableObjects/TaskSO", order = 2)]
public class TaskSO : ScriptableObject{

    public List<TaskData> TaskList;

    public void ClearTasks() {
        TaskList.Clear(); 
    }
}
[Serializable]
public class TaskData {
    public int index;
    public string Name;
    public string Description;
    public enum Type { Programmer, Artist, Designer }
    public Type TypeTask;
    public float Time;
    public bool IsAssigned = false;
    public bool IsComplete = false;
}
