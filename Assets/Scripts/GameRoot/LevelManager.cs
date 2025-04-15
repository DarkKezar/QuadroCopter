using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DroneScripts;
using UnityEngine.SceneManagement;
using System;

public enum GameMode 
{ 
    CheckPoints,
    Calibration
};

public enum Location
{
    Garage,
    SchoolGym,
    Playground,
    ParkRacingTrack,
    GarageRacingTrack,
    Nature,
    Industrial
}

public class LevelManager : MonoBehaviour
{
    public LevelsData levelsData;

    private string _levelName;

    private GetData _levelData;

    private GameMode _gameMode;
    private string _gameModeName;

    private Location _location;
    private string _locationName;
    
    private int _level;

    public static string LevelName { get; set; }
    public static GameMode GameMode { get; private set; }
    public static Location Location { get; private set; }
    public static int Level { get; private set; }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        DroneEventManager.onRequestsDone.AddListener(AnalyseRequest);
    }

    private void AnalyseRequest()
    {
        _levelData = DroneRequests.getData;

        _gameMode = (GameMode)_levelData.gamemode_enum;
        _gameModeName = _levelData.game_mode;
        GameMode = _gameMode;

        _location = (Location)_levelData.location_enum;
        _locationName = _levelData.location;
        Location = _location;

        _level = _levelData.level;
        Level = _level;

        SetLevelName();
    }

    private void SetLevelName()
    {
        _levelName = string.Format("{0}{1}_{2}", (int)_gameMode, (int)_location, _level);

#if UNITY_EDITOR || PLATFORM_STANDALONE_WIN
        _gameMode = GameMode.CheckPoints;
        _location = Location.Garage;
        _level = 1;
        _levelName = string.Format("{0}{1}_{2}", (int)_gameMode, (int)_location, _level);
#endif
        LevelName = _levelName;

        Debug.Log("Try to load Level: " + _levelName);

        foreach (var levelData in levelsData.levels)
        {
            if (levelData.gameMode == _gameMode && levelData.location == _location)
            {
                if (levelData.numOfLevels < _level)
                {
                    Debug.LogError("Level " + _level + " is unavailable" + "\n" +
                        "Game have " + levelData.numOfLevels + " Levels for GameMode " + _gameMode +
                        " and Location " + _location);
                    return;
                }
            }
        }
#if UNITY_EDITOR || PLATFORM_STANDALONE_WIN
        SceneManager.LoadScene("SelectLevel");
#elif PLATFORM_WEBGL
        SceneManager.LoadScene("LoadingScreen");
#else  
        Debug.Log("Try to build for WebGL platform");
#endif
    }
}
