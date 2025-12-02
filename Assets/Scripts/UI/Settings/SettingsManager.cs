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

    private string _sfxRtpc = "SFX_Volume";
    private string _musicRtpc = "Music_Volume";

    public void Start()
    {
        _sfxSlider.minValue = 0f;
        _sfxSlider.maxValue = 100f;
        _musicSlider.minValue = 0f;
        _musicSlider.maxValue = 100f;

        _sfxSlider.value = 100f;
        _musicSlider.value = 100f;
    }

    public void OnShakeToggle()
    {
        ShakeEnabled = !ShakeEnabled;
    }

    public void OnSFXChanged()
    {
        AkUnitySoundEngine.SetRTPCValue(_sfxRtpc, _sfxSlider.value);
    }

    public void OnMusicChanged()
    {
        AkUnitySoundEngine.SetRTPCValue(_musicRtpc, _musicSlider.value);
    }
}
