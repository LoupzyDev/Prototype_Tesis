using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NpcDisplay : MonoBehaviour {

    [SerializeField] private List<GameObject> npcList;
    [SerializeField] private List<Image> npcColors;
    [SerializeField] private List<TextMeshProUGUI> npcNames;

    [SerializeField] private int npcInDisplay;
    [SerializeField] private List<Image> npcBarHps;
    [SerializeField] private List<Image> npcBarStamins;
    [SerializeField] private List<Image> npcBarMorals;
    [SerializeField] private List<Image> npcBarHappiness;
    [SerializeField] private List<TextMeshProUGUI> npcType;
    [SerializeField] private Gradient colorBar;

    
    private void Start() {

        npcInDisplay = npcList.Count;
        for (int i = 0; i < npcInDisplay; i++) {
            UpdateColorNpcs(i);
            UpdateNamesNpcs(i);
        }
    }

    private void Update() {
        for (int i = 0; i < npcInDisplay; i++) {
            UpdateAllBars(i);
            npcType[i].text = npcList[i].GetComponent<Npc>().role;
        }
    }
    public void UpdateBar(Image bar,float actualValue,float maxValue) {

        bar.fillAmount = actualValue/maxValue;
        bar.color = colorBar.Evaluate(actualValue / maxValue);
    }
    private void UpdateColorNpcs(int i) {
        Renderer npcRenderer = npcList[i].GetComponentInChildren<Renderer>();
        Color npcColor = npcRenderer.material.color;
        npcColors[i].color = npcColor;
    }

    private void UpdateNamesNpcs(int i) {

        npcNames[i].text = npcList[i].name;
    }
    private void UpdateAllBars(int i) {

        float hpActualValue = npcList[i].GetComponent<Npc>().hp;
        float staminaActualValue = npcList[i].GetComponent<Npc>().stamina;
        float moralActualValue = npcList[i].GetComponent<Npc>().moral;
        float happinessActualValue = npcList[i].GetComponent<Npc>().happiness;

        UpdateBar(npcBarHps[i], hpActualValue, 100);
        UpdateBar(npcBarStamins[i], staminaActualValue, 100);
        UpdateBar(npcBarMorals[i], moralActualValue, 100);
        UpdateBar(npcBarHappiness[i], happinessActualValue, 100);
    }

}
