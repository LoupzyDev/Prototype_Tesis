using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    [SerializeField] private TaskSO taskData;
    [SerializeField] private TaskData currentTaskOS;

    [SerializeField] private GameObject[] GameObjsTask;

    private TaskData currentTaskData;
    [SerializeField] private int TaskIndex;


    public void addTasks(int numTask, float minQlty)
    {
        for(int i = 0;i<numTask;i++)
        {
            GameObjsTask[i].SetActive(true);

        }
    }
} 
 