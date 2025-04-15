using DroneController;
using DroneScripts;
using System;
using UnityEngine;

[Serializable]
struct GameStats
{
    public int damageCount;
    public float timeCount;
    public int collisionsCount;
    public int checkPointsCount;
    public int allCheckPoints;
    public bool isLevelComplete;
    public string endTime;
}

public class GameStatsManager : MonoBehaviour
{
    private int _damageCount = default;
    private float _timeCount = default;
    private int _collisionsCount = default;
    private int _checkPointsCount = default;
    private int _allCheckPoints = default;
    private bool _gameOver = default;
    private string _endTime = default;

    public int DamageCount { get { return _damageCount; } }
    public float TimeCount { get { return _timeCount; } }
    public int CollisionsCount { get { return _collisionsCount; } }
    public int CheckPointsCount { get { return _checkPointsCount; } }
    public int AllCheckPoints { get { return _allCheckPoints; } }
    public bool GameOver { get { return _gameOver; } }
    public string EndTime { get { return _endTime; } }

    private ObjectiveManager _frameManager;

    void Start()
    {
        _frameManager = FindObjectOfType<ObjectiveManager>();
        _timeCount = 0f;
        _allCheckPoints = _frameManager.GetAllFrames();

        DroneEventManager.onDroneDestroy.AddListener(EndGame);
        DroneEventManager.onLevelPassed.AddListener(EndGame);
    }
    void Update()
    {
        _timeCount += Time.deltaTime;
    }

    private void EndGame()
    {
        DroneEventManager.onDroneDestroy.RemoveListener(EndGame);
        DroneEventManager.onLevelPassed.RemoveListener(EndGame);

        _collisionsCount = DamageManager.Instance.CollisionCount;
        _checkPointsCount = _frameManager.GetCompletedFrames();
        _damageCount = (100 - Convert.ToInt16(DamageManager.Instance.Health));
        _endTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        SaveGameStats();
    }

    private void SaveGameStats()
    {
        //GameStats gameStats = new()
        //{
        //    damageCount = DamageCount,
        //    timeCount = TimeCount,
        //    collisionsCount = CollisionsCount,
        //    checkPointsCount = CheckPointsCount,
        //    allCheckPoints = AllCheckPoints,
        //    isLevelComplete = _frameManager.isLevelComplete(),
        //    endTime = EndTime,
        //};

        DroneRequests.postData = new();
        
        if (QueryParamsManager.queryParams == null)
        {
            DroneRequests.postData.id = 0;
            DroneRequests.postData.type = 0;
        }
        else
        {
            DroneRequests.postData.id = QueryParamsManager.queryParams.id;
            DroneRequests.postData.type = QueryParamsManager.queryParams.type;
        }
        

        DateTime epoch = new DateTime(1970, 1, 1); // Unix Epoch
        DateTime now = DateTime.UtcNow; // Nowaday UTC time
        DroneRequests.postData.result.date = (int)(now - epoch).TotalSeconds;
        DroneRequests.postData.result.is_finished = _frameManager.isLevelComplete();
        DroneRequests.postData.result.time = Convert.ToInt16(_timeCount);
        DroneRequests.postData.result.damage.max = 100;
        DroneRequests.postData.result.damage.get = Convert.ToInt16(_damageCount);
        DroneRequests.postData.result.collisions = _collisionsCount;
        DroneRequests.postData.result.check_points.all = _frameManager.GetAllFrames();
        DroneRequests.postData.result.check_points.get = _checkPointsCount;


        string json = JsonUtility.ToJson(DroneRequests.postData);
        Debug.Log("POST: " + json);
    }
}
