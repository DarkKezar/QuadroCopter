using DroneController;
using DroneScripts;
using System;
using TMPro;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [Header("Game Over Panels")]
    [SerializeField] private GameObject gameUI;
    [SerializeField] private GameObject windows;
    [SerializeField] private GameObject successPanel;
    [SerializeField] private GameObject failPanel;

    private GameStatsManager gameStatsManager;
    private EventFunctions eventFunctions;
    private ObjectiveManager frameManager;
    [SerializeField] private GameObject inputManager;

    private TextMeshProUGUI damageText;
    private TextMeshProUGUI timeText;
    private TextMeshProUGUI collisionsText;
    private TextMeshProUGUI checkPointsText;

    void Start()
    {
        frameManager = FindObjectOfType<ObjectiveManager>();
        gameStatsManager = FindObjectOfType<GameStatsManager>();
        eventFunctions = FindObjectOfType<EventFunctions>();

        DroneEventManager.onDroneDestroy.AddListener(LevelFailed);
        DroneEventManager.onLevelPassed.AddListener(LevelPassed);
        DroneEventManager.onCollected.AddListener(UpdateStats);
    }

    void LevelFailed()
    {
        DroneEventManager.onLevelPassed.RemoveListener(LevelPassed);

        eventFunctions.toggleGameObject(failPanel);
        eventFunctions.toggleGameObject(gameUI);
        eventFunctions.toggleGameObject(windows);

        //eventFunctions.PauseGame(inputManager);
        AssignTextObjects(failPanel);

        UpdateStats();
    }
    
    void LevelPassed()
    {
        DroneEventManager.onDroneDestroy.RemoveListener(LevelFailed);

        eventFunctions.toggleGameObject(successPanel);
        eventFunctions.toggleGameObject(gameUI);
        eventFunctions.toggleGameObject(windows);
        
        //eventFunctions.PauseGame(inputManager);
        AssignTextObjects(successPanel);

        UpdateStats();
    }


    void AssignTextObjects(GameObject activePanel)
    {
        damageText = activePanel.transform.Find("TotalDamage/DamageText").GetComponent<TextMeshProUGUI>();
        timeText = activePanel.transform.Find("CompletedTime/TimeText").GetComponent<TextMeshProUGUI>();
        collisionsText = activePanel.transform.Find("Collisions/CollisionsText").GetComponent<TextMeshProUGUI>();
        checkPointsText = activePanel.transform.Find("Checkpoints/CheckPointsText").GetComponent<TextMeshProUGUI>();
    }

    void UpdateStats()
    {
        damageText.text = (100 - Convert.ToInt16(DamageManager.Instance.Health)).ToString();
        timeText.text = FormatTime(gameStatsManager.TimeCount);
        collisionsText.text = DamageManager.Instance.CollisionCount.ToString();
        checkPointsText.text = frameManager.GetCompletedFrames().ToString() + "/" + frameManager.GetAllFrames().ToString();
    }

    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time % 60F);
        return string.Format("{0:0}:{1:00}", minutes, seconds);
    }
}