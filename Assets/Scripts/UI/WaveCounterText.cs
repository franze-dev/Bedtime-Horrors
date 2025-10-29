using System;
using TMPro;
using UnityEngine;

public class WaveCounterText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _counterText;
    [SerializeField] private int _startWaveId = 0;
    private int _currentWaveId = 0;
    private WaveManager _manager;

    private void Awake()
    {
        EventProvider.Subscribe<INewWaveEvent>(OnWaveStart);
    }

    private void OnWaveStart(INewWaveEvent @event)
    {
        if (_currentWaveId < _manager.WavesCount-1)
        {
            _currentWaveId++;
            SetValue(_currentWaveId, _manager.WavesCount);
        }
    }

    private void Start()
    {
        ServiceProvider.TryGetService(out _manager);
        SetValue(_currentWaveId, _manager.WavesCount);
    }

    public void SetValue(int currentWaveAmount, int totalWaves)
    {
        _counterText.text = currentWaveAmount+1 + " / " + totalWaves;
    }
}
