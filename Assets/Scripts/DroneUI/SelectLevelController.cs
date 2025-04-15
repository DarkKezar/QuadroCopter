using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SelectLevelController : MonoBehaviour
{
    public TMP_Dropdown dropdownGameModes;
    public TMP_Dropdown dropdownLocations;
    public TMP_Dropdown dropdownLevels;

    private string _levelName;

    private void Start()
    {
        if (Time.timeScale != 1f || AudioListener.pause)
        {
            Time.timeScale = 1f;
            AudioListener.pause = false;
        }
            
        

        _levelName = "00_1";
    }

    public void ChangeLevel()
    {
        int gameMode = 0;
        if (dropdownGameModes != null)
            gameMode = dropdownGameModes.value;

        int location = dropdownLocations.value;
        int level = dropdownLevels.value + 1;
        _levelName = string.Format("{0}{1}_{2}", gameMode, location, level);
        LevelManager.LevelName = _levelName;
    }

    public void StartLevel()
    {
        SceneManager.LoadScene("LoadingScreen");
    }

    public void StartSpecificLevel(string levelName)
    {
        _levelName = string.Format("{0}{1}_{2}", 0, 0, 0);
        LevelManager.LevelName = _levelName;

        SceneManager.LoadScene("LoadingScreen");
    }
}
