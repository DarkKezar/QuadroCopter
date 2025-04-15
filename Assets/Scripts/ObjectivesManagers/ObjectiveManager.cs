using DroneScripts;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    private static ObjectiveManager _instance;
    public static ObjectiveManager Instance { get { return _instance; } }

    Renderer frameRenderer;

    public GameObject checkpointsParent;
    public Material[] materials = new Material[3];
    
    public List<GameObject> frames { get; private set; }
    private int currentFrameIndex = 0;
    private int totalFrames;
    private int completedFrames;

    public bool levelComplete;

    public int CompletedFrames { get; private set; }
    public bool IsLevelComplete { get; private set; }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        levelComplete = false;
        frames = new List<GameObject>();

        foreach (Transform child in checkpointsParent.transform)
        {
            frames.Add(child.gameObject);
        }

        totalFrames = frames.Count;
        UpdateFrameColors();
    }

    void UpdateFrameColors()
    {
        for (int i = 0; i < frames.Count; i++)
        {
            frameRenderer = frames[i].GetComponent<Renderer>();

            if (i == currentFrameIndex)
            {
                frameRenderer.sharedMaterial = materials[0];
            }
            else if (i == currentFrameIndex + 1)
            {
                frameRenderer.sharedMaterial = materials[1];
            }
            else
            {
                frameRenderer.sharedMaterial = materials[2];
            }
        }
    }

    public void PassThroughFrame(GameObject frame)
    {
        if (frames[currentFrameIndex] == frame)
        {
            frames[currentFrameIndex].SetActive(false);
            completedFrames++;
            currentFrameIndex++;

            if (currentFrameIndex >= totalFrames)
            {
                levelComplete = true;
                DroneEventManager.LevelPassed();
                DroneEventManager.GameOver();
            }
            else
            {
                //DroneEventManager.Collected();
                UpdateFrameColors();
            }
        }
    }

    public bool isLevelComplete() => levelComplete;

    public int GetCompletedFrames() => completedFrames;


    public int GetRemainingFrames() => totalFrames - completedFrames;

    public int GetAllFrames() => totalFrames;
}
