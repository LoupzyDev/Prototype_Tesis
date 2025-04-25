using TMPro;
using UnityEngine;

public class Task : MonoBehaviour {
    public string nameTask;
    public int index;
    public string descriptionTask;
    public string typeTask;
    public float timeTask;
    public bool isComplete;
    [SerializeField] bool isTutorial;

    [SerializeField] private TextMeshProUGUI taskNameUI;
    [SerializeField] private TextMeshProUGUI taskNameDescription;
    [SerializeField] private TextMeshProUGUI taskTypeUI;
    [SerializeField] private TextMeshProUGUI taskDescriptionUI;
    [SerializeField] private TextMeshProUGUI taskTime;


    public void UpdateTaskUI() {
        taskTime.text = Clock._instance.actualTime;
        taskNameUI.text = nameTask;
    }

    private void Update() {
        if (!TaskManager._instance.taskData.TaskList.Exists(task => task.index == index)) {
            gameObject.SetActive(false); 
        }
    }

    public void ShowDescription() { 
        taskNameDescription.text = nameTask;
        taskTypeUI.text = typeTask;
        taskDescriptionUI.text = descriptionTask;
        if (isTutorial) { 
            DialogueManager._instance.exitButton.SetActive(true);
        }
    }
}




