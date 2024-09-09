using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanvasManager : MonoBehaviour {
    [SerializeField] private GameObject contructionMenu;
    [SerializeField] private GameObject taskMenu;
    [SerializeField] private GameObject buttonsGameplay;

    // Referencias a los NPCs
    [SerializeField] private NpcController npc1;
    [SerializeField] private NpcController npc2;
    [SerializeField] private NpcController npc3;
    [SerializeField] private NpcController npc4;

    // Referencias a los TextMeshPro
    [SerializeField] private TextMeshProUGUI npc1StaminaText;
    [SerializeField] private TextMeshProUGUI npc2StaminaText;
    [SerializeField] private TextMeshProUGUI npc3StaminaText;
    [SerializeField] private TextMeshProUGUI npc4StaminaText;

    private void Update() {
        // Actualizar la UI con la stamina de los NPCs y el color del texto
        UpdateStaminaUI(npc1, npc1StaminaText);
        UpdateStaminaUI(npc2, npc2StaminaText);
        UpdateStaminaUI(npc3, npc3StaminaText);
        UpdateStaminaUI(npc4, npc4StaminaText);
    }

    private void UpdateStaminaUI(NpcController npc, TextMeshProUGUI staminaText) {
        int stamina = npc.getStamina();
        staminaText.text = "Stamina: " + stamina.ToString();

        // Cambiar color del texto basado en la stamina
        if (stamina > 60) {
            staminaText.color = Color.green;
        } else if (stamina > 30) {
            staminaText.color = Color.yellow; 
        } else {
            staminaText.color = Color.red; 
        }
    }

    public void turnOffContructionMenu() {
        contructionMenu.gameObject.SetActive(false);
        buttonsGameplay.gameObject.SetActive(true);
    }

    public void turnOnContructionMenu() {
        contructionMenu.gameObject.SetActive(true);
        buttonsGameplay.gameObject.SetActive(false);
    }

    public void turnOffTaskMenu() {
        taskMenu.gameObject.SetActive(false);
        buttonsGameplay.gameObject.SetActive(true);
    }

    public void turnOnTaskMenu() {
        taskMenu.gameObject.SetActive(true);
        buttonsGameplay.gameObject.SetActive(false);
    }
}
