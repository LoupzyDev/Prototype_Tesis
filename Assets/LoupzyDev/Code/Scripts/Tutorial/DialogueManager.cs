using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager _instance;

    [SerializeField] private List<string> narrativeList;
    [SerializeField] private List<string> npcDialogueList;

    [SerializeField] private TextMeshProUGUI narrativeTextMP;
    [SerializeField] private TextMeshProUGUI npcTextMP;

    [SerializeField] private GameObject narrativePanelNpc;
    [SerializeField] private GameObject textPanelNpc;

    private void Awake() {
        _instance = this;
    }
    public void UpdateNarrativeDialogue(int index) {
        narrativeTextMP.text = narrativeList[index];
    }

    public void UpdateNpcDialogue(int index) {
        npcTextMP.text = npcDialogueList[index];
    }
    public void TurnOffOn(bool narrative, bool npc) { 
        narrativePanelNpc.SetActive(narrative);
        textPanelNpc.SetActive(npc);
    }

}
