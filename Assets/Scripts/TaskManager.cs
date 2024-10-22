using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    [SerializeField] private TaskSO taskData;
    [SerializeField] private GameObject[] GameObjTask;

    private TaskData currentTaskData;
    [SerializeField] private int TaskIndex;

    private void Start()
    {
        currentTaskData = taskData.TaskList[TaskIndex];
    }


} 
