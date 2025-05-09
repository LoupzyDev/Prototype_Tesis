using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


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

    [SerializeField] private Image iconCircle;
    [SerializeField] private List<Color> colorList;


    public void UpdateTaskUI() {
        taskTime.text = Clock._instance.actualTime;
        taskNameUI.text = nameTask;
        ColorOfIcon(typeTask);
    }

    private void Update() {
        if (!TaskManager._instance.taskData.TaskList.Exists(task => task.index == index)) {
            gameObject.SetActive(false); 
        }
    }

    public void ColorOfIcon(string type)
    {
        switch (type)
        {
            case "Programmer":
                iconCircle.color = colorList[0];
                break;
            case "Artist":
                iconCircle.color = colorList[1];
                break;
            case "Designer":
                iconCircle.color = colorList[2];
                break;
        }
    }
    public void ColorOfText(string type)
    {
        switch (type)
        {
            case "Programmer":
                taskTypeUI.color = colorList[0];
                break;
            case "Artist":
                taskTypeUI.color = colorList[1];
                break;
            case "Designer":
                taskTypeUI.color = colorList[2];
                break;
        }
    }
    public void ShowDescription() { 
        taskNameDescription.text = nameTask;
        taskTypeUI.text = typeTask;
        ColorOfText(typeTask);
        taskDescriptionUI.text = descriptionTask;
        if (isTutorial) { 
            DialogueManager._instance.exitButton.SetActive(true);
        }
    }
}




