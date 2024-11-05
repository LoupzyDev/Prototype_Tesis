using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour {
    public static TaskManager _instance;

    [SerializeField] private TaskSO taskData; // Scriptable Object que contiene la lista de tareas
    [SerializeField] private GameObject[] gameObjsTask; // Array de objetos de tareas

    private void Awake() {
        _instance = this;
        ClearTaskList(); // Limpia la lista de tareas al iniciar
    }

    public void AddTasks(int actualDay, int numTask, float minQlty) {
        for (int i = 0; i < numTask && i < gameObjsTask.Length; i++) {
            GameObject taskObject = gameObjsTask[i];
            CreateTask(taskObject);
            taskObject.GetComponent<Task>().UpdateTaskUI();
        }
    }

    public void DeactivateAllTasks() {
        foreach (GameObject taskObject in gameObjsTask) {
            taskObject.SetActive(false);
        }
    }

    public TaskData GetAvailableTask(string npcRole) {
        foreach (TaskData task in taskData.TaskList) {
            if (task.TypeTask.ToString() == npcRole && !task.IsAssigned) {
                task.IsAssigned = true;
                return task;
            }
        }
        return null;
    }

    private void CreateTask(GameObject indexTask) {
        indexTask.SetActive(true);
        float randomTime = Random.Range(1f, 10f);

        string randomName = GetRandomName();
        TaskData.Type randomType = GetRandomType();
        string randomDescription = GetRandomDescription();

        Task dragHandler = indexTask.GetComponent<Task>();
        dragHandler.timeTask = randomTime;
        dragHandler.nameTask = randomName;
        dragHandler.typeTask = randomType.ToString();
        dragHandler.descriptionTask = randomDescription;

        TaskData newTask = new TaskData {
            Name = randomName,
            TypeTask = randomType,
            Time = randomTime,
            Description = randomDescription,
            IsAssigned = false,
            IsComplete = false
        };

        taskData.TaskList.Add(newTask);
    }

    private string GetRandomName() {
        string[] names = new string[] { "Programa una mecánica", "Diseña el UI", "Crea el mapa del mundo" };
        return names[Random.Range(0, names.Length)];
    }

    private TaskData.Type GetRandomType() {
        TaskData.Type[] types = (TaskData.Type[])System.Enum.GetValues(typeof(TaskData.Type));
        return types[Random.Range(0, types.Length)];
    }

    private string GetRandomDescription() {
        string[] descriptions = new string[] { "Trabaja", "Trabaja sin pago", "Hazlo rápido" };
        return descriptions[Random.Range(0, descriptions.Length)];
    }

    public void ClearTaskList() {
        taskData.ClearTasks(); // Llama al método de TaskSO para limpiar la lista
    }
}