using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WindowManager : MonoBehaviour
{
    public static WindowManager _instance;
    public Toggle fullscreenToggle;
    public TMP_Dropdown resolutionDropdown;

    private Resolution[] availableResolutions;
    private List<Resolution> aspect16by9Resolutions = new List<Resolution>();

    private void Awake()
    {
        _instance = this;
    }
    private void Start()
    {
        availableResolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < availableResolutions.Length; i++)
        {
            float aspect = (float)availableResolutions[i].width / availableResolutions[i].height;
            if (Mathf.Approximately(aspect, 16f / 9f))
            {
                aspect16by9Resolutions.Add(availableResolutions[i]);

                string option = availableResolutions[i].width + " x " + availableResolutions[i].height;
                options.Add(option);

                if (availableResolutions[i].width == Screen.currentResolution.width &&
                    availableResolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = aspect16by9Resolutions.Count - 1;
                }
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();


        fullscreenToggle.isOn = Screen.fullScreen;
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution selectedResolution = aspect16by9Resolutions[resolutionIndex];
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreen);
    }
}
