using UnityEngine;
using UnityEngine.UI;

public class HUD_Car : MonoBehaviour
{
    [SerializeField] private Slider boostSlider;

    private void OnEnable()
    {
        CarMovement.onBoostGaugeChanged += OnBoostGaugeChanged;
    }

    private void OnDisable()
    {
        CarMovement.onBoostGaugeChanged -= OnBoostGaugeChanged;
    }

    private void OnBoostGaugeChanged(float boostGauge)
    {
        boostSlider.value = boostGauge;
    }
}
