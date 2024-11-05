using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropSlot : MonoBehaviour, IDropHandler {

    public GameObject task;

    public void OnDrop(PointerEventData eventData) {
        if (!task)
        {
            task = DragHandler.taskDragging;
            if (task != null)
            {
                task.transform.SetParent(transform);
                task.transform.position = transform.position;
            }
        }

    }

    private void Update() {
        if (task != null && task.transform.parent != transform) {
            task = null;
        }
    }
}
