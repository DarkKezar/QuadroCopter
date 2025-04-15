using UnityEngine;

public class FrameStand : MonoBehaviour
{
    private ObjectiveManager frameManager;
    private GameObject frame;

    void Start()
    {
        frameManager = FindObjectOfType<ObjectiveManager>();
        frame = gameObject;
    }

    void OnTriggerEnter(Collider other)
    {
        frameManager.PassThroughFrame(gameObject);
    }
}