using UnityEngine;

public class LineRendererTransparency : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public CameraManager cameraManager;
    public GameObject mainCamera;
    public float maxDistance = 5f;
    public float minDistance = 1f;
    public float maxAlpha = 1f;
    public float minAlpha = 0f;

    void Update()
    {
        mainCamera = cameraManager._activeCamera;
        UpdateLineTransparency();
    }

    void UpdateLineTransparency()
    {
        if (lineRenderer == null || mainCamera == null) return;

        float distance = Vector3.Distance(mainCamera.transform.position, lineRenderer.GetPosition(0));

        float alpha = Mathf.InverseLerp(maxDistance, minDistance, distance);
        alpha = Mathf.Clamp01(alpha);

        Color lineColor = lineRenderer.startColor;
        lineColor.a = Mathf.Lerp(minAlpha, maxAlpha, alpha);

        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;
    }
}
