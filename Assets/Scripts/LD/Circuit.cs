using Dreamteck.Splines;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Circuit : MonoBehaviour
{
    [SerializeField] private List<Road> roads = new List<Road>();
    [SerializeField] private Transform spawnPoint;

    public delegate void OnCircuitFinished(CarMovement agent, Circuit circuit);
    public OnCircuitFinished onCircuitFinished;

    public delegate void OnCheckpointCrossed(CarMovement agent, Circuit circuit);
    public OnCheckpointCrossed onCheckpointCrossed;

    public float GetDistanceFromRoad(Transform car)
    {
        return roads.Min(e => e.GetDistanceToSpline(car));
    }

    public Vector3 GetClosestPointFromRoad(Transform car)
    {
        Vector3 closestPoint = Vector3.zero;
        float minDistance = float.MaxValue;

        foreach (var road in roads)
        {
            SplineComputer spline = road.GetSpline();
            SplineSample projection = spline.Project(car.position);
            float distanceFromSpline = Vector3.Distance(car.position, projection.position);
            if(distanceFromSpline <= minDistance)
            {
                minDistance = distanceFromSpline;
                closestPoint = projection.position;
            }
        }

        return closestPoint;
    }

    public Road GetClosestRoad(Transform car)
    {
        Road closestRoad = null;
        float minDistance = float.MaxValue;

        foreach (var road in roads)
        {
            SplineComputer spline = road.GetSpline();
            SplineSample projection = spline.Project(car.position);
            float distanceFromSpline = Vector3.Distance(car.position, projection.position);
            if (distanceFromSpline <= minDistance)
            {
                minDistance = distanceFromSpline;
                closestRoad = road;
            }
        }

        return closestRoad;
    }

    public Vector3 GetSplinePointInFront(float distance, Transform car)
    {
        Road road = GetClosestRoad(car);
        if (road)
        {
            int roadIndex = roads.IndexOf(road);
            SplineSample sampleClosest = road.GetSpline().Project(car.position);
            double percentClosest = sampleClosest.percent;
            double currentDistance = road.GetSpline().CalculateLength() * percentClosest;
            double targetDistance = currentDistance + distance;

            while (roadIndex < roads.Count - 1 && targetDistance > road.GetSpline().CalculateLength())
            {
                targetDistance -= road.GetSpline().CalculateLength();
                roadIndex++;
                road = roads[roadIndex];
            }

            float newPercent = Mathf.Clamp01((float)targetDistance / road.GetSpline().CalculateLength());
            SplineSample inFront = road.GetSpline().Evaluate(newPercent);

            return inFront.position;
        }

        return Vector3.zero;
    }

    public List<Road> GetRoads()
    {
        return roads; 
    }

    public void SpawnAgent(CarMovement agent)
    {
        agent.GetRigidbody().MovePosition(spawnPoint.position);
        agent.transform.position = spawnPoint.position;
        agent.car.transform.rotation = spawnPoint.rotation;
    }

    public void ResetCircuit()
    {
        foreach (var road in roads)
        {
            RandomizeSplinePoint randomize = road.GetComponent<RandomizeSplinePoint>();
            if (randomize != null)
            {
                randomize.Randomize();
            }
        }
    }

    public Transform GetCarSpawnPoint()
    {
        return spawnPoint;
    }
}
