using UnityEngine;
using System.Collections.Generic;
using Evaluation;

public class ObstacleTracker : MonoBehaviour
{
    [SerializeField] private int obstacleLayer;
    [SerializeField] private float maxObstaclePenaltyReward = -1f;

    [Header("References")]
    [SerializeField] private CarMovement agent;

    private int contactObstacleCount = 0;
    private List<GameObject> hitObstacles = new List<GameObject>();
    public bool IsInContactWithObstacles => contactObstacleCount > 0;

    public void ResetObstacleTracker()
    {
        contactObstacleCount = 0;
        hitObstacles.Clear();
    }

    private void Update()
    {
        EvaluationTests.SetAgentIsColliding(IsInContactWithObstacles);
    }

    private void FixedUpdate()
    {
        if(IsInContactWithObstacles)
        {
            agent.AddReward(maxObstaclePenaltyReward / agent.MaxStep);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == obstacleLayer)
        {
            contactObstacleCount++;
            if(!hitObstacles.Contains(collision.gameObject))
            {
                hitObstacles.Add(collision.gameObject);
                EvaluationTests.AddHitObjstacle();
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == obstacleLayer)
        {
            contactObstacleCount--;
            if(contactObstacleCount < 0)
            {
                contactObstacleCount = 0;
            }
        }
    }
}
