using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] GameObject contructionMenu;
    [SerializeField] GameObject taskMenu;
    [SerializeField] GameObject buttonsGameplay;





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
