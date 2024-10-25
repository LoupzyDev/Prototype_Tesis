using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TasksData", menuName = "ScriptableObjects/TaskSO", order = 2)]
public class TaskSO : ScriptableObject{
    public List<TaskData> TaskList;
}
[Serializable]
public class TaskData {
    public string Name;

    public string Description;

    public enum Type { Programmer, Artist, Designer }
    public Type TypeTask;

    public float Time;
}
