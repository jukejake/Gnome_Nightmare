using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;


public class SettingsMenu : MonoBehaviour {

    public AudioMixer audioMixer;
    public Dropdown resolutionDropdown;
    private Resolution[] resolutions;
    public Resolution resolution;
    public bool isFullscreen = false;

    private Setting_Info setting_Info;

    private void Awake() {
        setting_Info = GameObject.FindObjectOfType(typeof(Setting_Info)) as Setting_Info;
        if (setting_Info) {
            setting_Info.settingsMenu = this;
            setting_Info.ApplyThis();
        }
    }

    private void Start() {
        resolutions = Screen.resolutions;
        List<Resolution> resolutionsV2 = new List<Resolution>();


        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        int j = 0;

        for (int i = 0; i < resolutions.Length; i++) {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            if (i >= 1 && option != (resolutions[i-1].width + " x " + resolutions[i-1].height)) {
                j++;
                resolutionsV2.Add(resolutions[i]);
                options.Add(option);
                if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height) { currentResolutionIndex = j; }
            }
        }

        resolutions = resolutionsV2.ToArray();

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

    }


    public void SetVolume(float volume) {
        audioMixer.SetFloat("volume", volume);
        if (setting_Info) { setting_Info.Volume = volume; }
    }

    public void ToggleTimer(bool isOn) {

    }
    public void SetTimer(float volume) {

    }

    public void SetQuality(int qualityIndex) {
        QualitySettings.SetQualityLevel(qualityIndex);
        if (setting_Info) { setting_Info.qualityIndex = qualityIndex; }
    }

    public void SetFullscreen(bool _isFullscreen) {
        isFullscreen = _isFullscreen;
        Screen.fullScreen = _isFullscreen;
        if (setting_Info) { setting_Info.isFullscreen = isFullscreen; }
    }

    public void SetResolution(int resolutionIndex) {
        resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, isFullscreen);
        if (setting_Info) { setting_Info.resolution = resolution; }
    }
    public void SetResolution(Resolution r) {
        resolution = r;
        Screen.SetResolution(resolution.width, resolution.height, isFullscreen);
    }
    public void SetResolution() {
        Screen.SetResolution(resolution.width, resolution.height, isFullscreen);
    }

}
