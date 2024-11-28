using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanvasManager : MonoBehaviour {
    [SerializeField] private GameObject contructionMenu;
    [SerializeField] private GameObject pcMenu;
    [SerializeField] private GameObject buttonsGameplay;
    [SerializeField] private GameObject stadisticsMenu;
    [SerializeField] private GameObject taskMenu;


    public void turnPanels(GameObject panelTrue, GameObject panelFalse) { 
        panelTrue.SetActive(true);
        panelFalse.SetActive(false);
    }

    public void turnOffContructionMenu() {

        turnPanels(buttonsGameplay,contructionMenu);
    }

    public void turnOnContructionMenu() {

        turnPanels(contructionMenu,buttonsGameplay);
    }

    public void turnOffpcMenu() {

        turnPanels(buttonsGameplay,pcMenu);
    }

    public void turnOnpcMenu() {

        turnPanels(pcMenu, buttonsGameplay);
    }

    public void turnOnTaskMenu() {
        turnPanels(taskMenu,stadisticsMenu);
    }
    public void turnOnStadisitcsMenu() {
        turnPanels(stadisticsMenu, taskMenu);
    }
}
