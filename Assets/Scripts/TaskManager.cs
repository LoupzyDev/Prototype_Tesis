using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    [SerializeField] private TaskSO taskData;
    [SerializeField] private TaskData currentTaskOS;

    [SerializeField] private GameObject[] GameObjsTask;

    private TaskData currentTaskData;
    [SerializeField] private int TaskIndex;

    string[] names = new string[] { "Programa una mecánica", "Diseña el UI", "Crea el mapa del mundo" };
    string[] types = new string[] { "Programmer", "Artist", "Designer" };
    string[] descriptions = new string[] { "Vas a valer verga", "Vas a valer el doble de verga", "Vas a valer el triple de verga" };

    public void addTasks(int actualDay, int numTask, float minQlty) {
        for (int i = 0; i < numTask; i++) {
            GameObject taskObject = GameObjsTask[i];
            createTask(taskObject);

            // Llama a UpdateTaskUI después de configurar cada tarea
            taskObject.GetComponent<DragHandler>().UpdateTaskUI();
        }
    }
    public void DeactivateAllTasks() {
        foreach (GameObject taskObject in GameObjsTask) {
            taskObject.SetActive(false); // Desactiva cada tarea
        }
    }


    private void createTask(GameObject indexTask)
    {
        indexTask.SetActive(true);
        indexTask.GetComponent<DragHandler>().timeTask = Random.Range(1, 10);

        string randomName = names[Random.Range(0, names.Length)];
        string randomType = types[Random.Range(0, types.Length)];
        string randomDescription = descriptions[Random.Range(0, descriptions.Length)];

        indexTask.GetComponent<DragHandler>().nameTask = randomName;
        indexTask.GetComponent<DragHandler>().typeTask = randomType;
        indexTask.GetComponent<DragHandler>().descriptionTask = randomDescription;
    }

}
