using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour {
    [SerializeField] private GameObject contructionMenu;
    [SerializeField] private GameObject pcMenu;
    [SerializeField] private GameObject buttonsGameplay;
    [SerializeField] private GameObject stadisticsMenu;
    [SerializeField] private GameObject taskMenu;
    [SerializeField] private GameObject pauseMenu;

    private bool isPaused = false;

    void Update() {
        // Detecta si se presiona la tecla Escape
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isPaused) {
                turnOffPauseMenu();
            } else {
                turnOnPauseMenu();
            }
            isPaused = !isPaused; // Alterna el estado de pausa
        }
    }

    public void turnPanels(GameObject panelTrue, GameObject panelFalse) {
        panelTrue.SetActive(true);
        panelFalse.SetActive(false);
    }

    public void turnOffContructionMenu() {
        turnPanels(buttonsGameplay, contructionMenu);
    }

    public void turnOnContructionMenu() {
        turnPanels(contructionMenu, buttonsGameplay);
    }

    public void turnOffpcMenu() {
        turnPanels(buttonsGameplay, pcMenu);
    }

    public void turnOnpcMenu() {
        turnPanels(pcMenu, buttonsGameplay);
    }

    public void turnOnTaskMenu() {
        turnPanels(taskMenu, stadisticsMenu);
    }

    public void turnOnStadisitcsMenu() {
        turnPanels(stadisticsMenu, taskMenu);
    }

    public void turnOnPauseMenu() {
        pauseMenu.SetActive(true);
        pcMenu.SetActive(false);
        Time.timeScale = 0.0f; // Pausa el juego
    }

    public void turnOffPauseMenu() {
        pauseMenu.SetActive(false);
        Time.timeScale = 1.0f; // Reanuda el juego
        buttonsGameplay.SetActive(true);
    }
    public void RestartScene() {
        Time.timeScale = 1.0f; // Asegúrate de reanudar el tiempo antes de reiniciar
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void QuitGame() {
        Application.Quit();
    }
}
