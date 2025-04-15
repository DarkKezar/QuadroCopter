using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelConfig
{
    public GameMode gameMode;
    public Location location;
    public int numOfLevels;

    public LevelConfig(GameMode gameMode, Location location, int numOfLevels)
    {
        this.gameMode = gameMode;
        this.location = location;
        this.numOfLevels = numOfLevels;
    }
}


[CreateAssetMenu(fileName = "LevelsData", menuName = "ScriptableObjects/LevelsData", order = 1)]
public class LevelsData : ScriptableObject
{
    public LevelConfig[] levels;
}
