using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    public static VolumeManager _instance;
    public AudioMixer audioMixer;
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider vfxVolumeSlider;

    private const string masterVolume = "MasterVolume";
    private const string musicVolume = "MusicVolume";
    private const string vfxVolume = "VFXVolume";

    private void Awake()
    {
        _instance = this;
    }
    void Start()
    {
        float savedMasterVolume = PlayerPrefs.GetFloat(masterVolume, 0.1f);
        masterVolumeSlider.value = savedMasterVolume;
        ApplyVolume(masterVolume, savedMasterVolume);
        masterVolumeSlider.onValueChanged.AddListener(value => ApplyVolume(masterVolume, value));

        float savedVFXVolume = PlayerPrefs.GetFloat(vfxVolume, 0.8f);
        vfxVolumeSlider.value = savedVFXVolume;
        ApplyVolume(vfxVolume, savedVFXVolume);
        vfxVolumeSlider.onValueChanged.AddListener(value => ApplyVolume(vfxVolume, value));

        float savedMusicVolume = PlayerPrefs.GetFloat(musicVolume, 0.8f);
        musicVolumeSlider.value = savedMusicVolume;
        ApplyVolume(musicVolume, savedMusicVolume);
        musicVolumeSlider.onValueChanged.AddListener(value => ApplyVolume(musicVolume, value));
    }

    public void ApplyVolume(string parameterName, float linearVolume)
    {
        float clampedVolume = Mathf.Clamp(linearVolume, 0.0001f, 1f);
        float dB = Mathf.Log10(clampedVolume) * 20f;

        audioMixer.SetFloat(parameterName, dB);
        PlayerPrefs.SetFloat(parameterName, clampedVolume);
    }
}
