using Dreamteck.Splines;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct PointModification
{
    public int pointIndex;
    public bool useRandomSign;
    public Vector3 minPositionLocalOffset;
    public Vector3 maxPositionLocalOffset;
    [HideInInspector] public Vector3 baseLocalPosition;
}

[Serializable]
public struct CheckpointSnapData
{
    public double percent;
    public Vector3 positionOffset;
}

public class RandomizeSplinePoint : MonoBehaviour
{
    [SerializeField] private SplineComputer splineComputer;
    [SerializeField] private List<SplineComputer> splinesToCopyTo;
    [SerializeField] private List<PointModification> pointsModification = new List<PointModification>();

    private bool isInit = false;
    private Road road;
    private List<CheckpointSnapData> checkpointsSnapData = new List<CheckpointSnapData>();

    void Init()
    {
        road = GetComponent<Road>();
        if (road == null)
        {
            Debug.LogWarning($"No road found for RandomizeSplinePoint of {gameObject.name}");
            return;
        }

        for (int i = 0; i < pointsModification.Count; i++)
        {
            PointModification pointModification = pointsModification[i];
            pointModification.baseLocalPosition = splineComputer.GetPointPosition(pointModification.pointIndex, SplineComputer.Space.Local);
            pointsModification[i] = pointModification;
        }

        List<Checkpoint> checkpointsToSnap = road.GetCheckpoints();
        for (int i = 0; i < checkpointsToSnap.Count; i++)
        {
            SplineSample projection = splineComputer.Project(checkpointsToSnap[i].transform.position);
            CheckpointSnapData snapData = new CheckpointSnapData();
            snapData.percent = projection.percent;
            snapData.positionOffset = checkpointsToSnap[i].transform.position - projection.position;
            checkpointsSnapData.Add(snapData);
        }

        isInit = true;
    }

    public void Randomize()
    {
        if(!isInit)
        {
            Init();
        }

        StartCoroutine(Cor_Randomize());
    }

    IEnumerator Cor_Randomize()
    {
        RandomizeSpline();
        yield return null; //wait for spline meshes update
        SnapCheckpoints();
    }

    private void RandomizeSpline()
    {
        for (int i = 0; i < pointsModification.Count; i++)
        {
            float randomX = UnityEngine.Random.Range(pointsModification[i].minPositionLocalOffset.x, pointsModification[i].maxPositionLocalOffset.x) * (pointsModification[i].useRandomSign ? RandomSign() : 1.0f);
            float randomY = UnityEngine.Random.Range(pointsModification[i].minPositionLocalOffset.y, pointsModification[i].maxPositionLocalOffset.y) * (pointsModification[i].useRandomSign ? RandomSign() : 1.0f);
            float randomZ = UnityEngine.Random.Range(pointsModification[i].minPositionLocalOffset.z, pointsModification[i].maxPositionLocalOffset.z) * (pointsModification[i].useRandomSign ? RandomSign() : 1.0f);
            
            Vector3 offset = new Vector3(randomX, randomY, randomZ);
            Vector3 newPointPosition = pointsModification[i].baseLocalPosition + offset;
            splineComputer.SetPointPosition(pointsModification[i].pointIndex, newPointPosition, SplineComputer.Space.Local);

            foreach(SplineComputer copy in splinesToCopyTo)
            {
                copy.SetPointPosition(pointsModification[i].pointIndex, newPointPosition, SplineComputer.Space.Local);
            }
        }
    }

    private float RandomSign()
    {
        return UnityEngine.Random.value > 0.5f ? 1.0f : -1.0f;
    }

    private void SnapCheckpoints()
    {
        List<Checkpoint> checkpointsToSnap = road.GetCheckpoints();
        for (int i = 0; i < checkpointsToSnap.Count; i++)
        {
            CheckpointSnapData snapData = checkpointsSnapData[i];
            SplineSample sample = splineComputer.Evaluate(snapData.percent);
            checkpointsToSnap[i].transform.position = sample.position + snapData.positionOffset;
            checkpointsToSnap[i].transform.rotation = Quaternion.LookRotation(sample.forward, sample.up);
        }
    }
}
