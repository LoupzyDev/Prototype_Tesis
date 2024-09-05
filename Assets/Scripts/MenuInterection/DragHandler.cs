using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler {

    public static GameObject taskDragging;
    public string taskType;
    public float timeTask;
    
    public Vector3 startPosition;
    public Transform startParent;
    private Transform dragParent;
    private CanvasGroup canvasGroup;

    private void Start() {
        dragParent = GameObject.FindGameObjectWithTag("DragParent").transform;
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData) {
        taskDragging = gameObject;
        startPosition = transform.position;
        startParent = transform.parent;
        transform.SetParent(dragParent);
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData) {
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, Input.mousePosition, eventData.pressEventCamera, out globalMousePos)) {
            rectTransform.position = globalMousePos;
        }
    }

    public void OnEndDrag(PointerEventData eventData) {
        taskDragging = null;

        if (transform.parent == dragParent) {
            transform.position = startPosition;
            transform.SetParent(startParent);
        }

        canvasGroup.blocksRaycasts = true;
    }
}
