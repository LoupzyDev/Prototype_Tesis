using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager _instance;
    [SerializeField] private List<Sprite> mouseSprites;

    [SerializeField] private List<string> narrativeList;
    [SerializeField] private List<string> npcDialogueList;

    [SerializeField] private TextMeshProUGUI narrativeTextMP;
    [SerializeField] private TextMeshProUGUI npcTextMP;

    [SerializeField] private GameObject narrativePanelNpc;
    [SerializeField] private GameObject textPanelNpc;
    [SerializeField] private Image mouseImage;

    [SerializeField] private GameObject windowsPanel;
    public GameObject exitButton;
    private void Awake() {
        _instance = this;
    }
    public void UpdateNarrativeDialogue(int index) {
        narrativeTextMP.text = narrativeList[index];
    }

    public void UpdateNpcDialogue(int index) {
        npcTextMP.text = npcDialogueList[index];
    }
    public void TurnOffOnTextPanel(bool textNarrative, bool textNpc, bool mouseIcon) { 
        narrativePanelNpc.SetActive(textNarrative);
        textPanelNpc.SetActive(textNpc);
        mouseImage.gameObject.SetActive(mouseIcon);
    }
    public void UpdateIcon(int index) {
        mouseImage.sprite = mouseSprites[index];
    }

    public void SwichPanelWindow(bool isActive) {
        TurnOffOnTextPanel(false, false, false);
        if (isActive) {
            windowsPanel.SetActive(isActive);
        } else {
            windowsPanel.SetActive(isActive);
            StateManager._instance.ChangeState(State.MoveToDesk);
        }
        
        StateManager._instance.SwichIconWindow(false, false);
        BoxCheck._instance.panelMission.SetActive(false);
    }
}
