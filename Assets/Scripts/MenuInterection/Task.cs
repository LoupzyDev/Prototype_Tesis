using TMPro;
using UnityEngine;

public class Task : MonoBehaviour {
    public string nameTask;
    public int index;
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
    }

    private void Update() {

        if (!TaskManager._instance.taskData.TaskList.Exists(task => task.index == index)) {
            gameObject.SetActive(false); 
        }
    }

    public void ShowDescription() {
        taskDescriptionUI.text = descriptionTask;
    }
}




