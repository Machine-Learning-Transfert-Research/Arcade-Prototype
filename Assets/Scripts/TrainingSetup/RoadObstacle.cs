using Dreamteck.Splines;
using Evaluation;
using System.Collections.Generic;
using UnityEngine;

public class RoadObstacle : MonoBehaviour
{
    [SerializeField] private int obstacleSpawnRate = 25;
    [SerializeField] private float obstacleDistance = 40f;
    [SerializeField] private int offsetMaxSize = 5;
    [SerializeField] private GameObject obstaclePrefabs;

    private List<GameObject> obstacles = new List<GameObject>();
    private Transform carSpawnPoint;

    public void SetObstacle(Circuit circuit)
    {
        carSpawnPoint = circuit.GetCarSpawnPoint();

        for (int i = 0; i < circuit.GetRoads().Count; i++)
        {
            SplineComputer spline = circuit.GetRoads()[i].GetSpline();
            double carSpawnPointPercent = i == 0 ? spline.Project(carSpawnPoint.position).percent : 0f;
            float splineLenght = spline.CalculateLength();

            for (float spawnDist = obstacleDistance; spawnDist < splineLenght; spawnDist += obstacleDistance)
            {
                if (obstacleSpawnRate >= Random.Range(0, 101))
                {
                    double percent = spawnDist / splineLenght + carSpawnPointPercent;
                    SplineSample splineSample = spline.Evaluate(percent > 1 ? 1 : percent);
                    Vector3 position = splineSample.position;
                    int offsetSize = Random.Range(0, offsetMaxSize + 1);
                    int offsetDirection = Random.Range(0, 2);

                    if (offsetDirection == 0)
                        position += splineSample.right * offsetSize;
                    else
                        position -= splineSample.right * offsetSize;

                    obstacles.Add(Instantiate(obstaclePrefabs, position, Quaternion.Euler(0, Random.Range(0, 360), 0), transform));
                }
            }
        }

        EvaluationTests.SetMaxObstacle(obstacles.Count);
    }

    public void DeleteObstacle()
    {
        for (int i = 0; i < obstacles.Count; i++)
            Destroy(obstacles[i]);
        obstacles.Clear();
    }
}
