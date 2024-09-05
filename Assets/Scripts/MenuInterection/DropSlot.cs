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

                string taskType = task.GetComponent<DragHandler>().taskType;

                // Ejecuta la función correspondiente usando el número del NPC asignado a este DropSlot
                ExecuteTask(assignedNpcNumber, taskType);
            }
        } else {
            // Si ya hay una tarea asignada, la tarea arrastrada vuelve a su posición original
            DragHandler.taskDragging.transform.position = DragHandler.taskDragging.GetComponent<DragHandler>().startPosition;
            DragHandler.taskDragging.transform.SetParent(DragHandler.taskDragging.GetComponent<DragHandler>().startParent);
        }
    }

    private void ExecuteTask(int npcNumber, string taskType) {
        float taskDuration = task.GetComponent<DragHandler>().timeTask; // Obtener la duración de la tarea

        switch (npcNumber) {
            case 0:
                if (taskType == "Work") {
                    GameManager._instance.StartNpcTask(0, taskDuration, NpcState.Working);
                } else if (taskType == "Play") {
                    GameManager._instance.StartNpcTask(0, taskDuration, NpcState.Playing);
                }
                break;
            case 1:
                if (taskType == "Work") {
                    GameManager._instance.StartNpcTask(1, taskDuration, NpcState.Working);
                } else if (taskType == "Play") {
                    GameManager._instance.StartNpcTask(1, taskDuration, NpcState.Playing);
                }
                break;
            case 2:
                if (taskType == "Work") {
                    GameManager._instance.StartNpcTask(2, taskDuration, NpcState.Working);
                } else if (taskType == "Play") {
                    GameManager._instance.StartNpcTask(2, taskDuration, NpcState.Playing);
                }
                break;
            case 3:
                if (taskType == "Work") {
                    GameManager._instance.StartNpcTask(3, taskDuration, NpcState.Working);
                } else if (taskType == "Play") {
                    GameManager._instance.StartNpcTask(3, taskDuration, NpcState.Playing);
                }
                break;

            default:
                Debug.Log("Unknown NPC or task type");
                break;
        }
    }


    private void Update() {
        if (task != null && task.transform.parent != transform) {
            task = null;
        }
    }
}
