using Evaluation;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.InferenceEngine;
using Unity.MLAgents.Policies;
using UnityEngine;
using static EnvTraining;
using Random = UnityEngine.Random;

public class EnvTraining : MonoBehaviour
{
    [SerializeField] private List<Circuit> circuits;
    [SerializeField] private CarMovement agent;
    [SerializeField] private RoadObstacle roadObstacle;

    [Header("Rewards")]
    [SerializeField] private float failPenalty = -1.0f;


    private int maxCircuitID = 0;
    private Circuit currentCircuit = null;
    public Circuit Circuit { get { return currentCircuit; } }

    public delegate void OnCircuitUpdated(Circuit circuit);
    public OnCircuitUpdated onCircuitUpdated;

    public void Awake()
    {
        foreach (Circuit circuit in circuits)
        {
            circuit.gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        if(EvaluationTests.GetEventOnTrialStart() != null)
        {
            EvaluationTests.GetEventOnTrialStart().AddListener(SetupAgentModel);
        }

        if (EvaluationTests.GetEventTimeTrialEnd() != null)
        {
            EvaluationTests.GetEventTimeTrialEnd().AddListener(OnTimeTrialEnd);
        }
    }

    private void OnDestroy()
    {
        if (EvaluationTests.GetEventOnTrialStart() != null)
        {
            EvaluationTests.GetEventOnTrialStart().RemoveListener(SetupAgentModel);
        }

        if (EvaluationTests.GetEventTimeTrialEnd() != null)
        {
            EvaluationTests.GetEventTimeTrialEnd().RemoveListener(OnTimeTrialEnd);
        }
    }

    private void OnDisable()
    {
        ChangeCircuit(null);
        if(roadObstacle)
        {
            roadObstacle.DeleteObstacle();
        }
    }

    private void SetupAgentModel(ModelAsset model)
    {
        agent.GetComponent<BehaviorParameters>().Model = model;
    }

    public void ResetEnvTraining()
    {
        if (roadObstacle)
        {
            roadObstacle.DeleteObstacle();
        }
        
        int circuitID = Random.Range(0, maxCircuitID + 1);
        ChangeCircuit(circuits[circuitID]);

        currentCircuit.ResetCircuit();

        if (roadObstacle)
        {
            StartCoroutine(Cor_AddObstacle());
        }

        currentCircuit.SpawnAgent(agent);
    }

    private void ChangeCircuit(Circuit newCircuit)
    {
        if (currentCircuit == newCircuit) return;

        if(currentCircuit != null)
        {
            currentCircuit.onCircuitFinished -= OnCircuitFinished;
            currentCircuit.gameObject.SetActive(false);
        }

        currentCircuit = newCircuit;
        onCircuitUpdated?.Invoke(currentCircuit);

        if (currentCircuit != null)
        {
            currentCircuit.gameObject.SetActive(true);
            currentCircuit.onCircuitFinished += OnCircuitFinished;
        }
    }

    public void OnCircuitFinished(CarMovement agent, Circuit circuit)
    {
        int indexCircuit = circuits.IndexOf(circuit);
        if(indexCircuit == maxCircuitID)
        {
            maxCircuitID = Mathf.Clamp(indexCircuit + 1, 0, circuits.Count - 1);
        }

        EndEpisode(true);
    }

    void OnTimeTrialEnd()
    {
        EndEpisode(false);
    }

    public void EndEpisode(bool success)
    {
        EvaluationTests.FinishTrial(!success);
        if(!success)
        {
            agent.SetReward(failPenalty);
        }

        agent.EndEpisode();
    }

    IEnumerator Cor_AddObstacle()
    {
        yield return null; //need to wait for the initialization of the spline meshes
        roadObstacle.SetObstacle(currentCircuit);
    }
}
