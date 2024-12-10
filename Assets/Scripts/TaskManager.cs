using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TaskManager : MonoBehaviour {
    public static TaskManager _instance;

    public TaskSO taskData; // Scriptable Object que contiene la lista de tareas
    [SerializeField] private GameObject[] gameObjsTask; // Array de objetos de tareas

    private TaskJsonData taskJsonData;

    [SerializeField] private int maxDailyTasks; // Número máximo de tareas por día
    private int currentTaskCount = 0; // Contador de tareas creadas

    private void Awake() {
        _instance = this;
        LoadTaskDataFromJson();
        ClearTaskList(); 
    }

    private void LoadTaskDataFromJson() {
        string filePath = Path.Combine(Application.streamingAssetsPath, "taskProperties.json");
        string jsonContent = File.ReadAllText(filePath);
        taskJsonData = JsonUtility.FromJson<TaskJsonData>(jsonContent);
    }

    public void StartAddingTasks(int actualDay,int numOfTask ,float minQlty) {
        StartCoroutine(AddTasksPeriodically(actualDay, numOfTask, minQlty));
    }

    private IEnumerator AddTasksPeriodically(int actualDay, int numTask, float minQlty) {
        currentTaskCount = 0;

        while (currentTaskCount < numTask && currentTaskCount < gameObjsTask.Length) {
            GameObject taskObject = gameObjsTask[currentTaskCount];
            CreateTask(taskObject,currentTaskCount);
            taskObject.GetComponent<Task>().UpdateTaskUI();
            currentTaskCount++;

            yield return new WaitForSeconds(5f); 
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

    private void CreateTask(GameObject indexTask,int currentIndex) {
        indexTask.SetActive(true);
        float randomTime = Random.Range(1f, 10f);

        string randomName = GetRandomFromList(taskJsonData.names);
        TaskData.Type randomType = GetRandomFromEnum<TaskData.Type>();
        string randomDescription = GetRandomFromList(taskJsonData.descriptions);

        Task taskObject = indexTask.GetComponent<Task>();
        taskObject.timeTask = randomTime;
        taskObject.index = currentIndex;
        taskObject.nameTask = randomName;
        taskObject.typeTask = randomType.ToString();
        taskObject.descriptionTask = randomDescription;

        TaskData newTask = new TaskData {
            Name = randomName,
            index=currentIndex,
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
        taskData.ClearTasks(); 
    }
    public void RemoveCompletedTasks() {
        for (int i = taskData.TaskList.Count - 1; i >= 0; i--) {
            if (taskData.TaskList[i].IsComplete) {
                GameObject taskObject = gameObjsTask[i];
                if (taskObject != null) {
                    taskObject.SetActive(false);
                }
                taskData.TaskList.RemoveAt(i);
            }
        }
    }



}

[System.Serializable]
public class TaskJsonData {
    public List<string> names;
    public List<string> types;
    public List<string> descriptions;
}
