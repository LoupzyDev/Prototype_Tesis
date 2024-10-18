using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    [SerializeField] private TaskSO task;
    [SerializeField] private GameObject[] GameObjTask;

    private void Start()
    {
        string hola = "Me cago en todo ";
        task.Name = hola;
        task.Time = 4;

        updateTasks(1);
    }

    void updateTasks(int numberOfTask)
    {

        for(int i= 0; i < numberOfTask; i++)
        {
            GameObjTask[i].GetComponent<DragHandler>().taskType = task.Name;
            GameObjTask[i].GetComponent<DragHandler>().timeTask = task.Time;
        }
    }
} 
