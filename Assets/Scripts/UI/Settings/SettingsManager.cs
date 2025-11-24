using System;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private Toggle _shakeCheckbox;

    public static bool ShakeEnabled = true;

    public void ApplyChanges()
    {
        InitSlider(_musicSlider, OnMasterChanged);
        InitSlider(_sfxSlider, OnSFXChanged);

        _shakeCheckbox.onValueChanged.AddListener(OnShakeToggle);
    }

    private void OnShakeToggle(bool isOn)
    {
        ShakeEnabled = isOn;
    }

    private void OnSFXChanged(float volume)
    {
        throw new NotImplementedException();
    }

    private void OnMasterChanged(float volume)
    {
        throw new NotImplementedException();
    }

    private void InitSlider(Slider slider, UnityAction<float> callback)
    {
        slider.wholeNumbers = true;
        slider.minValue = 0;
        slider.maxValue = 100;

        slider.onValueChanged.AddListener(callback);
    }
}
