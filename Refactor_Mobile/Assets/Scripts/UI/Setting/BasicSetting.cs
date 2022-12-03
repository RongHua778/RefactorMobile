using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Linq;

public class BasicSetting : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Dropdown resolutionDropdown;
    public Toggle fullScreenToggle;
    List<Resolution> resolutions;
    [SerializeField] Slider musicSlider = default;
    [SerializeField] Slider effectSlider = default;

    public void Initialize()
    {
        InitializeResolution();
        ShowSetting();
    }

    public void ShowSetting()
    {
        fullScreenToggle.isOn = Screen.fullScreen;
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1);
        effectSlider.value= PlayerPrefs.GetFloat("EffectVolume", 1);
    }

    private void InitializeResolution()
    {
        resolutions = Screen.resolutions.Select(resolution => new Resolution { width = resolution.width, height = resolution.height }).Distinct().ToList();
        resolutionDropdown.ClearOptions();

        Resolution resolution = new Resolution();
        resolution.width = 2560;
        resolution.height = 1080;
        resolution.refreshRate = 60;
        resolutions.Add(resolution);

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Count; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        if (PlayerPrefs.GetInt("Resolution", 0) != 0)
        {
            resolutionDropdown.value = PlayerPrefs.GetInt("Resolution", 0);
        }
        else
        {
            resolutionDropdown.value = currentResolutionIndex;
        }
        resolutionDropdown.RefreshShownValue();
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVolume", volume);
    }

    public void SetSoundVolume(float volume)
    {
        audioMixer.SetFloat("effectVolume", volume);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("Resolution", resolutionIndex);
    }


    public void SaveSetting()
    {
        StaticData.SetTipsPos();
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("EffectVolume", effectSlider.value);
    }


}
