using UnityEngine;
using UnityEngine.Events;

public class CarTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent onCarEnter;
    [SerializeField] private UnityEvent onCarExit;

    private void OnTriggerEnter(Collider other)
    {
        CarMovement car = other.GetComponent<CarMovement>();
        if (car == null)
        {
            return;
        }

        onCarEnter?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        CarMovement car = other.GetComponent<CarMovement>();
        if (car == null)
        {
            return;
        }

        onCarExit?.Invoke();
    }
}
