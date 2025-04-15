using DroneController;
using TMPro;
using UnityEngine;

public class FrameHandler : MonoBehaviour
{
    
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI completedText;
    public TextMeshProUGUI allText;

    public float timeElapsed;
    private ObjectiveManager _frameManager;

    void Start()
    {
        _frameManager = FindObjectOfType<ObjectiveManager>();
        timeElapsed = 0f;
    }

    void Update()
    {
        if (_frameManager.GetRemainingFrames() != 0 && !DamageManager.Instance.Broken)
            timeElapsed += Time.deltaTime;
        timerText.text = "" + FormatTime(timeElapsed);

        completedText.text = "" + _frameManager.GetCompletedFrames();
        allText.text = "" + _frameManager.GetAllFrames();
    }

    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time % 60F);
        return string.Format("{0:0}:{1:00}", minutes, seconds);
    }
}
