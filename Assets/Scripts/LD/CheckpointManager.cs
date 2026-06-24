using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CarMovement agent;

    [Header("Rewards")]
    [SerializeField] private float maxRewardCheckpoints = 1.0f;
    [SerializeField] private float multiplierCheckpointRewardOffroad = 0.5f;
    [SerializeField] private float maxRewardCompletion = 1.0f;

    private int currentCheckpointIndex = 0;
    private List<Checkpoint> checkpoints = new List<Checkpoint>();

    public int CheckpointCount { get { return checkpoints.Count; } }

    void Awake()
    {
        FillCheckpointsList();
    }

    public void FillCheckpointsList()
    {
        if (agent.GetCurrentCircuit() == null) return;

        checkpoints.Clear();
        foreach (var roads in agent.GetCurrentCircuit().GetRoads())
        {
            checkpoints.AddRange(roads.GetCheckpoints());
        }
        currentCheckpointIndex = 0;
    }

    public void ResetCheckpointManager()
    {
        currentCheckpointIndex = 0;
    }

    public void OnCheckpointCrossed(Checkpoint checkpoint)
    {
        if (checkpoint == null) return;

        int checkpointIndex = checkpoints.IndexOf(checkpoint);
        if (checkpointIndex < currentCheckpointIndex)
        {
            return;
        }

        currentCheckpointIndex = checkpointIndex + 1;
        if(agent.GetGroundID() == 2)
        {
            agent.AddReward(maxRewardCheckpoints * multiplierCheckpointRewardOffroad / checkpoints.Count);
        }
        else
        {
            agent.AddReward(maxRewardCheckpoints / checkpoints.Count);
        }

        Circuit currentCircuit = agent.GetCurrentCircuit();
        currentCircuit.onCheckpointCrossed.Invoke(agent, currentCircuit);

        if (currentCheckpointIndex >= checkpoints.Count)
        {
            agent.AddReward(maxRewardCompletion);
            Debug.Log("Cumulative Reward = " + agent.GetCumulativeReward());
            currentCircuit.onCircuitFinished.Invoke(agent, currentCircuit);
        }
    }

    public Checkpoint GetNextCheckpoint()
    {
        if (currentCheckpointIndex < checkpoints.Count)
        {
            return checkpoints[currentCheckpointIndex];
        }

        return null;    
    }
}
