using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum State {
    None,
    ShowDialogue,
    ClickNpc,
    Move,
    OpenWindows,
    MoveToDesk,
    TaskComplete
}

public class StateManager : MonoBehaviour {
    public static StateManager _instance;
    public List<GameObject> remainingNps;
    private State currentState = State.None;
    private int returnIndex;
    [SerializeField] private Material outlineNpc;
    [SerializeField] private Material outlineTable;
    [SerializeField] private Material outlineDesk;
    [SerializeField] private GameObject selectionManager;
    [SerializeField] private GameObject box;
    [SerializeField] private GameObject buttonWindow;
    [SerializeField] private Image iconsAlert;
    [SerializeField] private List<Color> colorAlerts;

    [SerializeField] private DaySO daysDataSO;
    [SerializeField] private DayData currentDay;
    [SerializeField] private GameObject desk;
    [SerializeField] private GameObject npc;

    private int numberOfTasks_GM;
    private float minQuality_GM;
    private void Awake() {
        Time.timeScale = 1f;
        _instance = this;
        outlineNpc.SetFloat("_Outline_Thickness", 0.0f);
        outlineNpc.SetColor("_OutlineColor", Color.white);
    }
    void Start() {
        returnIndex = 0;
        ChangeState(State.ShowDialogue);
    }

    void Update() {

        if(Input.GetKeyDown(KeyCode.P))
        {
            changeAnotherScene();
        }

        if (Input.GetMouseButtonDown(0) && currentState == State.ShowDialogue) {
            ChangeState(State.ClickNpc);
        } else if (Input.GetMouseButtonDown(0) && currentState == State.ClickNpc && returnIndex == 0) {
            DialogueManager._instance.TurnOffOnTextPanel(false, false, true);
            DialogueManager._instance.UpdateIcon(0);
            outlineNpc.SetFloat("_Outline_Thickness", 0.0002f);
            outlineNpc.SetColor("_OutlineColor", Color.red);
            selectionManager.GetComponent<NpcSelectionManager>().enabled = true;
            returnIndex++;
        } else if (Input.GetMouseButtonDown(0) && currentState == State.ClickNpc && returnIndex == 1) {
            ChangeState(State.Move);
            box.SetActive(true);
            DialogueManager._instance.TurnOffOnTextPanel(false, false, false);
            returnIndex++;
        } else if (currentState == State.Move && returnIndex == 1 && BoxCheck._instance.isEnd) {
            outlineNpc.SetFloat("_Outline_Thickness", 0.0f);
            outlineNpc.SetColor("_OutlineColor", Color.white);
            ChangeState(State.OpenWindows);
        } else if (npc.GetComponent<NpcMovement>().enabled && currentState == State.MoveToDesk && returnIndex == 0) {
            DialogueManager._instance.deskImage.gameObject.SetActive(true);
        } else if (desk.GetComponent<Desk>().IsOccupied() && currentState == State.MoveToDesk && returnIndex == 0) {
            DialogueManager._instance.TurnOffOnTextPanel(false, false, false);
            returnIndex++;
        } else if (currentState == State.MoveToDesk && returnIndex == 1) {
            DialogueManager._instance.deskImage.gameObject.SetActive(false);
            DialogueManager._instance.TurnOffOnTextPanel(false, false, true);
            DialogueManager._instance.UpdateIcon(1);
            returnIndex++;
        } else if (currentState == State.MoveToDesk && returnIndex == 2 && desk.GetComponent<Desk>().IsOccupied() == false) {
            DialogueManager._instance.TurnOffOnTextPanel(true, false, true);
            DialogueManager._instance.UpdateNarrativeDialogue(1);
            DialogueManager._instance.UpdateIcon(2);
            ChangeState(State.TaskComplete);
        } else if (currentState == State.TaskComplete && returnIndex == 1) {
            npc.GetComponent<NpcMovement>().enabled = false;
            returnIndex++;
        } else if (currentState == State.TaskComplete && returnIndex == 2 && remainingNps[0].GetComponent<NpcMovement>().agent.hasPath==false) {
            DialogueManager._instance.TurnOnNpcPresentation(true);
        }
    }

    public void ChangeState(State newState) {
        if (currentState != newState) {
            Debug.Log($"Changing state to: {newState}");
            currentState = newState;
            HandleState();
        }
    }

    void HandleState() {
        returnIndex = 0;
        switch (currentState) {

            case State.None:
                break;

            case State.ShowDialogue:
                DialogueManager._instance.TurnOffOnTextPanel(true,false,false);
                DialogueManager._instance.UpdateNarrativeDialogue(0);
                break;

            case State.ClickNpc:
                DialogueManager._instance.TurnOffOnTextPanel(false, true,false);
                DialogueManager._instance.UpdateNpcDialogue(0);
                break;

            case State.Move:
                break;

            case State.OpenWindows:
                DialogueManager._instance.TurnOffOnTextPanel(false, true, false);
                DialogueManager._instance.UpdateNpcDialogue(1);
                updateTask(0);
                buttonWindow.SetActive(true);
                iconsAlert.color = colorAlerts[0];
                break;

            case State.MoveToDesk:
                DialogueManager._instance.TurnOffOnTextPanel(false, false, true);
                outlineTable.SetFloat("_OutlineWidth", 50f);
                DialogueManager._instance.UpdateIcon(0);
                break;

            case State.TaskComplete:
                foreach (var npc in remainingNps) {
                    npc.gameObject.SetActive(true);
                    outlineTable.SetFloat("_OutlineWidth", 0f);
                    NpcSelectionManager._instance.npcsSelect.Add(npc.gameObject);
                    npc.GetComponent<Npc>().state = NpcState.Walking;
                }
                returnIndex++;
                break;
        }
    }
    public void updateTask(int dayIndex) {
        currentDay = daysDataSO.days[dayIndex];
        numberOfTasks_GM = currentDay.numberOfTasks;
        minQuality_GM = currentDay.minQuality;

        TaskManager._instance.StartAddingTasks(dayIndex , numberOfTasks_GM, minQuality_GM);

    }

    public void SwichIconWindow(bool button, bool icons) {
        buttonWindow.SetActive(button);
        if(icons) {
            iconsAlert.color = colorAlerts[0];
        } else {
            iconsAlert.color = colorAlerts[1];
        }
    }

    public void changeAnotherScene() {
        SceneManager.LoadScene("Level_1");
    }
}
