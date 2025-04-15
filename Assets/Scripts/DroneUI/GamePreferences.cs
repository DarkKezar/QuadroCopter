using UnityEngine;
using System;

[Serializable]
public class GamePreferences
{
    [Serializable]
    public class SerializableResolution
    {
        public int width;
        public int height;

        public SerializableResolution() { }

        public SerializableResolution(Resolution resolution)
        {
            width = resolution.width;
            height = resolution.height;
        }

        public SerializableResolution(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public Resolution ToResolution()
        {
            return new Resolution { width = width, height = height };
        }
    }


    public SerializableResolution CurrentResolution;
    public bool IsFullScreen;
    public float Volume;
    public bool Tutorial;
    public string Quality;
    public bool PostProcessing;
    public bool AutoCalibration;
    public bool MinimalUiElements;

    public GamePreferences()
    {
        CurrentResolution = new SerializableResolution(1920, 1080);
        IsFullScreen = true;
        Volume = 1f;
        Tutorial = true;
        Quality = "2";
        PostProcessing = true;
        AutoCalibration = true;
        MinimalUiElements = false;
    }

    public Resolution GetResolution()
    {
        return CurrentResolution.ToResolution();
    }
}
