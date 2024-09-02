using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TaskPool : MonoBehaviour, IDropHandler {

    public void OnDrop(PointerEventData eventData) {
        if (DragHandler.taskDragging == null) return;


        DragHandler.taskDragging.transform.SetParent(transform);
        DragHandler.taskDragging.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
    }
}
