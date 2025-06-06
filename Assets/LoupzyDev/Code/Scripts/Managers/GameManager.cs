using System.Collections.Generic;
using TMPro;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager _instance;
    [SerializeField] private List<Npc> npcControllers;

    [SerializeField] private DaySO daysDataSO;
    [SerializeField] private DayData currentDay;

    private int numberOfTasks_GM;
    private float minQuality_GM;

    [SerializeField] private Clock clock;
    [SerializeField] private TaskManager taskManager;

    public int playerMoney;
    [SerializeField] private TextMeshProUGUI moneyText;

    [SerializeField] private ObjectsDatabaseSO database; 
    [SerializeField] private List<TextMeshProUGUI> furniturePriceTMPList;

    private void Awake() {
        _instance = this;
    }
    private void Start() {
        UpdateMoneyUI();
        UpdateAllFurniturePrices();
    }

    public void SetAllNpcsToSleeping() {
        foreach (Npc npcController in npcControllers) {
            npcController.ChangeGameState(NpcState.Sleeping); 
        }
    }
    public void SetAllNpcsToWakeUp() {
        foreach (Npc npcController in npcControllers) {
            npcController.gameObject.SetActive(true);
            npcController.ChangeGameState(NpcState.Walking); 
        }
    }
   
    public void updateTask(int dayIndex)
    {
        currentDay = daysDataSO.days[dayIndex];
        numberOfTasks_GM=currentDay.numberOfTasks;
        minQuality_GM=currentDay.minQuality;

        TaskManager._instance.StartAddingTasks(dayIndex + 1, numberOfTasks_GM, minQuality_GM);

    }

    public bool CanAfford(int amount) {
        return playerMoney >= amount;
    }

    public void DeductMoney(int amount) {
        playerMoney -= amount;
        UpdateMoneyUI();
    }
    public void UpdateMoneyUI() {
        moneyText.text = $"Dinero: {playerMoney} Mxn"; 
    }
    public void UpdateAllFurniturePrices() {
        for (int i = 0; i < furniturePriceTMPList.Count; i++) {
            if (i < database.objectsData.Count) {
                int price = database.objectsData[i].Price;
                furniturePriceTMPList[i].text = $"Precio: {price} Mxn";
            } else {
                furniturePriceTMPList[i].text = "No disponible"; 
            }
        }
    }
}
