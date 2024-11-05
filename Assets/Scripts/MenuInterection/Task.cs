using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Task : MonoBehaviour {

    public static GameObject taskDragging;


    public Vector3 startPosition;
    public Transform startParent;
    private Transform dragParent;
    private CanvasGroup canvasGroup;

    public string nameTask;
    public string descriptionTask;
    public string typeTask;
    public float timeTask;
    public bool isComplete;

    [SerializeField] private TextMeshProUGUI taskNameUI;
    [SerializeField] private TextMeshProUGUI taskTypeUI;
    [SerializeField] private TextMeshProUGUI taskDescriptionUI;


    public void UpdateTaskUI() {
        taskNameUI.text = nameTask;
        taskTypeUI.text = typeTask;
        taskDescriptionUI.text = descriptionTask;
    }

    private void Update() {
        if (isComplete) {
            gameObject.SetActive(false);
        }
    }
}