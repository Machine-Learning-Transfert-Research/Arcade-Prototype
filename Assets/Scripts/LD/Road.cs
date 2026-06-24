using Dreamteck.Splines;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Road : MonoBehaviour
{
    [SerializeField] private SplineComputer spline;
    [SerializeField] private Transform checkpointHolder;
    [SerializeField] private bool autoFillCheckpointsList = true;
    [SerializeField] private List<Checkpoint> checkpoints = new List<Checkpoint>();

    private void OnValidate()
    {
        if(autoFillCheckpointsList)
        {
            AutoFillCheckpointList();
        }
    }

    private void Awake()
    {
        if (autoFillCheckpointsList)
        {
            AutoFillCheckpointList();
        }
    }

    private void AutoFillCheckpointList()
    {
        checkpoints.Clear();
        checkpoints = checkpointHolder.GetComponentsInChildren<Checkpoint>().ToList();
    }

    public float GetDistanceToSpline(Transform car)
    {
        SplineSample projection = spline.Project(car.transform.position);
        return Vector3.Distance(car.transform.position, projection.position);
    }

    public List<Checkpoint> GetCheckpoints()
    {
        return checkpoints;
    }

    public SplineComputer GetSpline()
    {
        return spline; 
    }
}
