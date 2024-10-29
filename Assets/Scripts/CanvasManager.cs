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
