using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TaskManager : MonoBehaviour {
    public static TaskManager _instance;

    [SerializeField] private TaskSO taskData; // Scriptable Object que contiene la lista de tareas
    [SerializeField] private GameObject[] gameObjsTask; // Array de objetos de tareas

    private TaskJsonData taskJsonData;

    private void Awake() {
        _instance = this;
        LoadTaskDataFromJson();
        ClearTaskList(); // Limpia la lista de tareas al iniciar
    }

    private void LoadTaskDataFromJson() {
        string filePath = Path.Combine(Application.streamingAssetsPath, "taskProperties.json");
        string jsonContent = File.ReadAllText(filePath);
        taskJsonData = JsonUtility.FromJson<TaskJsonData>(jsonContent);


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

        string randomName = GetRandomFromList(taskJsonData.names);
        TaskData.Type randomType = GetRandomFromEnum<TaskData.Type>();
        string randomDescription = GetRandomFromList(taskJsonData.descriptions);

        Task taskObject = indexTask.GetComponent<Task>();
        taskObject.timeTask = randomTime;
        taskObject.nameTask = randomName;
        taskObject.typeTask = randomType.ToString();
        taskObject.descriptionTask = randomDescription;

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

    private string GetRandomFromList(List<string> list) {
        return list[Random.Range(0, list.Count)];
    }

    private T GetRandomFromEnum<T>() {
        T[] values = (T[])System.Enum.GetValues(typeof(T));
        return values[Random.Range(0, values.Length)];
    }

    public void ClearTaskList() {
        taskData.ClearTasks(); // Llama al método de TaskSO para limpiar la lista
    }
}

[System.Serializable]
public class TaskJsonData {
    public List<string> names;
    public List<string> types;
    public List<string> descriptions;
}
