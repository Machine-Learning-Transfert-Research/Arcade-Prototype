using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        CheckpointManager checkpointManager = other.GetComponent<CheckpointManager>();
        if (checkpointManager != null)
        {
            checkpointManager.OnCheckpointCrossed(this);
        }
    }
}
