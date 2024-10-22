using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager _instance;
    [SerializeField] private List<NpcController> npcControllers;

    [SerializeField] private DaySO daysDataSO;
    [SerializeField] private DayData currentDay;

    private int numberOfTasks_GM;
    private float minQuality_GM;

    [SerializeField] private Clock clock;
    [SerializeField] private TaskManager taskManager;

    private void Awake() {
        _instance = this;
    }


    public void StartNpcTask(int npcIndex, float duration, NpcState taskState) {
        npcControllers[npcIndex].StartTask(duration, taskState);
    }

   
    public void SetAllNpcsToSleeping() {
        foreach (NpcController npcController in npcControllers) {
            npcController.ChangeGameState(NpcState.Sleeping); // Cambia el estado a Sleeping
        }
    }
    public void SetAllNpcsToWakeUp() {
        foreach (NpcController npcController in npcControllers) {
            npcController.gameObject.SetActive(true);
            npcController.ChangeGameState(NpcState.Walking); // Cambia el estado a Walking
        }
    }
   
    public void updateTask(int dayIndex)
    {
        currentDay = daysDataSO.days[dayIndex];
        numberOfTasks_GM=currentDay.numberOfTasks;
        minQuality_GM=currentDay.minQuality;

        Debug.Log(numberOfTasks_GM + " " +  minQuality_GM);
        taskManager.addTasks(numberOfTasks_GM, minQuality_GM);
    }
}
