using UnityEngine;

public class CollectibleObject : MonoBehaviour
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