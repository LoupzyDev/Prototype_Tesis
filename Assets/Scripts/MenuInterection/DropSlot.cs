using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropSlot : MonoBehaviour, IDropHandler {

    public GameObject task;
    private List<NpcController> listNpcs;
    public int indexNpc;

    private float timeToComplete;
    private void Start()
    {
        listNpcs = GameManager._instance.npcControllers;
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (!task)
        {
            task = DragHandler.taskDragging;
            if (task != null)
            {
                if (listNpcs[indexNpc].role == task.GetComponent<DragHandler>().typeTask)
                {
                    timeToComplete = task.GetComponent<DragHandler>().timeTask;
                    GameManager._instance.StartNpcTask(indexNpc, timeToComplete, NpcState.Working);
                    task.transform.SetParent(transform);
                    task.transform.position = transform.position;
                }
                
            }
        }

    }

    private void Update() {
        if (task != null && task.transform.parent != transform) {
            task = null;
        }
    }
}
