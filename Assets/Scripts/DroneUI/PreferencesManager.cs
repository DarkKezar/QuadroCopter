using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using System;

public class PreferencesManager : MonoBehaviour
{
    private GamePreferences gamePreferences;
    private string saveFilePath;

    private Resolution[] resolutions;
    public TMP_Dropdown resolutionsDropdown;
    public Toggle fullscreenToggle;
    public Slider volumeSlider;
    public Toggle tutorialToggle;
    public List<Toggle> qualityToggles;
    public List<Toggle> additionalSettings;
    void Start()
    {
        gamePreferences = new GamePreferences();
        saveFilePath = Path.Combine(Application.persistentDataPath, "GamePreferences.json");
        resolutions = Screen.resolutions;

        LoadPreferences();
        SavePreferences();
    }

    void PopulateResolutionDropdown()
    {
        resolutionsDropdown.ClearOptions();
        List<string> options = new List<string>();

        foreach (Resolution res in resolutions)
        {
            options.Add(res.width + " x " + res.height);
        }

        resolutionsDropdown.AddOptions(options);
        resolutionsDropdown.value = GetCurrentResolutionIndex();
        resolutionsDropdown.RefreshShownValue();
    }

    int GetCurrentResolutionIndex()
    {
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].width == gamePreferences.CurrentResolution.width &&
                resolutions[i].height == gamePreferences.CurrentResolution.height)
            {
                return i;
            }
        }
        return 0;
    }

    void LoadPreferences()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            gamePreferences = JsonUtility.FromJson<GamePreferences>(json);
        }

        PopulateResolutionDropdown();

        fullscreenToggle.isOn = gamePreferences.IsFullScreen;
        volumeSlider.value = gamePreferences.Volume;
        tutorialToggle.isOn = gamePreferences.Tutorial;

        foreach (Toggle toggle in qualityToggles)
        {
            toggle.isOn = toggle.name.Equals(gamePreferences.Quality);
        }

        foreach (Toggle toggle in additionalSettings)
        {
            if (toggle.name.Equals("PostProcessing"))
            {
                toggle.isOn = gamePreferences.PostProcessing;
            }
            else if (toggle.name.Equals("AutoCalibration"))
            {
                toggle.isOn = gamePreferences.AutoCalibration;
            }
            else if (toggle.name.Equals("MinimalUiElements"))
            {
                toggle.isOn = gamePreferences.MinimalUiElements;
            }
            else
            {
                toggle.isOn = false;
            }
        }

        ApplyPreferences();
    }

    public void ApplyPreferences()
    {
        Resolution selectedResolution = resolutions[resolutionsDropdown.value];
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, fullscreenToggle.isOn);
        AudioListener.volume = volumeSlider.value;

        gamePreferences.CurrentResolution = new GamePreferences.SerializableResolution(Screen.currentResolution);
        gamePreferences.IsFullScreen = fullscreenToggle.isOn;
        gamePreferences.Volume = volumeSlider.value;
        gamePreferences.Tutorial = tutorialToggle.isOn;

        foreach (Toggle toggle in qualityToggles)
        {
            if (toggle.isOn)
            {
                gamePreferences.Quality = toggle.name;
                QualitySettings.SetQualityLevel(Convert.ToInt16(gamePreferences.Quality));
            }
        }

        foreach (Toggle toggle in additionalSettings)
        {
            if (toggle.name.Equals("PostProcessing"))
            {
                gamePreferences.PostProcessing = toggle.isOn;
            }
            else if (toggle.name.Equals("AutoCalibration"))
            {
                gamePreferences.AutoCalibration = toggle.isOn;
            }
            else if (toggle.name.Equals("MinimalUiElements"))
            {
                gamePreferences.MinimalUiElements = toggle.isOn;
            }
        }
    }

    public void SavePreferences()
    {
        ApplyPreferences();

        string json = JsonUtility.ToJson(gamePreferences, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Preferences saved to: " + saveFilePath);
    }
}