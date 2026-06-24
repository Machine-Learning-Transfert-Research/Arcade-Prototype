using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class CarCamera : MonoBehaviour
{
    [SerializeField] private Transform target;

    [Header("Settings")]
    [SerializeField] private Vector3 offset = new Vector3(0, 2.5f, -5.0f);
    [SerializeField] private float translationSpeed = 10f;
    [SerializeField] private float rotationSpeed = 5f;

    private void LateUpdate()
    {
        HandleCamera();
    }

    private void HandleCamera()
    {
        if (!target) return;

        Vector3 targetPosition = target.TransformPoint(offset);
        transform.position = Vector3.Lerp(transform.position, targetPosition, translationSpeed * Time.deltaTime);

        Vector3 direction = target.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }

    public void Warp()
    {
        Vector3 targetPosition = target.TransformPoint(offset);
        transform.position = targetPosition;

        Vector3 direction = target.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = targetRotation;
    }
}
