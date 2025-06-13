using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager _instance;
    [SerializeField] private GameObject constructionMenu;
    [SerializeField] private GameObject pcMenu;
    [SerializeField] private GameObject buttonsGameplay;
    [SerializeField] private GameObject statisticsMenu;
    [SerializeField] private GameObject taskMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject exitButton;

    private bool isPaused = false;


    private void Awake()
    {
        _instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }
    private void TogglePauseMenu()
    {
        isPaused = !isPaused;
        SetPauseState(isPaused);
    }
    public void ResumeGame()
    {
        isPaused = false;
        SetPauseState(false);
    }

    private void SetPauseState(bool paused)
    {
        pauseMenu.SetActive(paused);
        Time.timeScale = paused ? 0f : 1f;

        pcMenu.SetActive(paused);
        taskMenu.SetActive(!paused);
        statisticsMenu.SetActive(false);
        exitButton.SetActive(!paused);
        buttonsGameplay.SetActive(!paused);
    }   

    private void SwitchPanels(GameObject toEnable, GameObject toDisable)
    {
        toEnable.SetActive(true);
        toDisable.SetActive(false);
    }

    public void ShowConstructionMenu()
    {
        SwitchPanels(constructionMenu, buttonsGameplay);
    }

    public void HideConstructionMenu()
    {
        SwitchPanels(buttonsGameplay, constructionMenu);
    }

    public void ShowPcMenu()
    {
        SwitchPanels(pcMenu, buttonsGameplay);
    }

    public void HidePcMenu()
    {
        SwitchPanels(buttonsGameplay, pcMenu);
    }

    public void ShowTaskMenu()
    {
        SwitchPanels(taskMenu, statisticsMenu);
    }

    public void ShowStatisticsMenu()
    {
        SwitchPanels(statisticsMenu, taskMenu);
    }

    public void ChangeLevel(string levelName)
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(levelName);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

}