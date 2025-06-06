using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public  static MainMenu _instance;
    public GameObject mainMenuPanel;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject windowPanel;
    [SerializeField] private GameObject soundPanel;

    private void Awake()
    {
        _instance = this;
    }
    private void Start()
    {
        OpenMainMenuPanel();
    }

    public void ChangeLevel(string levelName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(levelName);
    }

    public void OpenOptionsPanel()
    {
        turnOffAllPanels();
        optionsPanel.SetActive(true);
    }
    public void ExitGame()
    {
        Application.Quit();
    }

    public void OpenWindowPanel()
    {
        turnOffAllPanels();
        optionsPanel.SetActive(true);
        windowPanel.SetActive(true);
    }
    public void OpenSoundPanel()
    {
        turnOffAllPanels();
        optionsPanel.SetActive(true);
        soundPanel.SetActive(true);
    }
    public void OpenMainMenuPanel()
    {
        turnOffAllPanels();
        mainMenuPanel.SetActive(true);
    }

    private void turnOffAllPanels()
    {
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(false);
        windowPanel.SetActive(false);
        soundPanel.SetActive(false);
    }
}
