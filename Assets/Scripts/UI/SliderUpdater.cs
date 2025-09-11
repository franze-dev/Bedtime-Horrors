using UnityEngine;
using UnityEngine.UI;

public class SliderUpdater : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    public void UpdateSlider(float currentValue, float maxValue)
    {
        _slider.value = currentValue / maxValue;
    }

}