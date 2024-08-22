using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler {

    public static GameObject taskDragging;
    private Vector3 startPosition;
    private Transform startParent;
    private Transform dragParent;
    private CanvasGroup canvasGroup;

    private void Start() {
        dragParent = GameObject.FindGameObjectWithTag("DragParent").transform;
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData) {
        Debug.Log("OnBeginDrag");
        taskDragging = gameObject;

        startPosition = transform.position;
        startParent = transform.parent;
        transform.SetParent(dragParent);

        // Hace transparente el objeto arrastrado para mejorar la visualizaci�n
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData) {
        Debug.Log("Drag");

        // Usa RectTransform para la posici�n de UI
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, Input.mousePosition, eventData.pressEventCamera, out globalMousePos)) {
            rectTransform.position = globalMousePos;
        }
    }

    public void OnEndDrag(PointerEventData eventData) {
        Debug.Log("OnEndDrag");
        taskDragging = null;

        // Si no se dej� caer en una ranura v�lida, regresa a la posici�n original
        if (transform.parent == dragParent) {
            transform.position = startPosition;
            transform.SetParent(startParent);
        }

        // Restaura la capacidad de hacer clic en el objeto
        canvasGroup.blocksRaycasts = true;
    }
}
