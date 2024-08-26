using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropSlot : MonoBehaviour, IDropHandler {

    public GameObject task;
    public int assignedNpcNumber; // Número del NPC al que este DropSlot asignará la tarea

    public void OnDrop(PointerEventData eventData) {
        if (!task) {
            task = DragHandler.taskDragging;
            if (task != null) {
                task.transform.SetParent(transform);
                task.transform.position = transform.position;

                // Obtén el tipo de tarea
                string taskType = task.GetComponent<DragHandler>().taskType;

                // Ejecuta la función correspondiente usando el número del NPC asignado a este DropSlot
                ExecuteTask(assignedNpcNumber, taskType);
            }
        }
    }

    private void ExecuteTask(int npcNumber, string taskType) {
        switch (npcNumber) {
            case 0:
                if (taskType == "Work") {
                    TaskWork();
                    GameManager._instance.ChangeNpcStateByIndex(0, 2);
                } else if (taskType == "Play") {
                    TaskPlay();
                    GameManager._instance.ChangeNpcStateByIndex(0, 1);
                }
                break;
            case 1:
                if (taskType == "Work") {
                    TaskWork();
                    GameManager._instance.ChangeNpcStateByIndex(1, 2);
                } else if (taskType == "Play") {
                    TaskPlay();
                    GameManager._instance.ChangeNpcStateByIndex(1, 1);
                }
                break;
            case 2:
                if (taskType == "Work") {
                    TaskWork();
                    GameManager._instance.ChangeNpcStateByIndex(2, 2);
                } else if (taskType == "Play") {
                    TaskPlay();
                    GameManager._instance.ChangeNpcStateByIndex(2, 1);
                }
                break;
            case 3:
                if (taskType == "Work") {
                    TaskWork();
                    GameManager._instance.ChangeNpcStateByIndex(3, 2);
                } else if (taskType == "Play") {
                    TaskPlay();
                    GameManager._instance.ChangeNpcStateByIndex(3, 1);
                }
                break;

            // Agrega más casos según los NPCs y tipos de tareas que tengas
            default:
                Debug.Log("Unknown NPC or task type");
                break;
        }
    }

    private void TaskWork() {
        // Código específico para la tarea de trabajo
        Debug.Log("Executed Task Type: Work");
    }

    private void TaskPlay() {
        // Código específico para la tarea de juego
        Debug.Log("Executed Task Type: Play");
    }

    private void Update() {
        if (task != null && task.transform.parent != transform) {
            task = null;
        }
    }
}
