using DroneController;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class PathRenderer : MonoBehaviour
{
    public List<Vector3> _points;
    public LineRenderer _lineRenderer;
    public ObjectiveManager _objectiveManager;
    public Vector3 _droneSpawnPoint;
    [SerializeField] private GameObject _drone;
    [SerializeField] private GameObject _endPoint;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private int resolution = 10;

    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();

        _objectiveManager = ObjectiveManager.Instance;
        if (_objectiveManager == null)
        {
            Debug.Log(_objectiveManager + " is null");
        }
        
        _droneSpawnPoint = _drone.transform.position;
    }

    void UpdatePoints()
    {

        _points = new List<Vector3>(_objectiveManager.GetRemainingFrames() + 2)
        {
            _drone.transform.position + _offset,
            _drone.transform.position + _offset,
        };
        foreach (Transform child in _objectiveManager.checkpointsParent.transform)
        {
            if (child.gameObject.activeSelf)
                _points.Add(child.position);
        }
        _points.Add(_endPoint.transform.position);
    }


    void FixedUpdate()
    {
        UpdatePoints();
        DrawCurve();
    }
    void DrawCurve()
    {
        if (_points.Count < 4)
        {
            _lineRenderer.positionCount = 0;
            return;
        }
            

        List<Vector3> smoothPoints = new List<Vector3>();

        for (int i = 0; i <= _points.Count - 4; i++)
        {
            Vector3 p0 = _points[i];
            Vector3 p1 = _points[i + 1];
            Vector3 p2 = _points[i + 2];
            Vector3 p3 = _points[i + 3];

            for (int j = 0; j < resolution; j++)
            {
                float t = j / (float)resolution;
                Vector3 point = CatmullRom(p0, p1, p2, p3, t);
                smoothPoints.Add(point);
            }
        }

        DrawLines(smoothPoints);
    }

    void DrawLines(List<Vector3> smoothPoints)
    {
        _lineRenderer.positionCount = smoothPoints.Count;
        _lineRenderer.SetPositions(smoothPoints.ToArray());
    }

    Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float t2 = t * t;
        float t3 = t2 * t;

        return 0.5f * ((2f * p1) +
                       (-p0 + p2) * t +
                       (2f * p0 - 5f * p1 + 4f * p2 - p3) * t2 +
                       (-p0 + 3f * p1 - 3f * p2 + p3) * t3);
    }
}
