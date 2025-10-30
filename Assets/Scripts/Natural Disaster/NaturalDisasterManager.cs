using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaturalDisasterManager : MonoBehaviour
{
    [SerializeField] private List<NaturalDisaster> _disasters;
    [SerializeField] private float _minInterval = 30f;
    [SerializeField] private float _maxInterval = 120f;
    private NaturalDisaster _currentDisaster;
    private WaveManager _waveManager;

    bool _isCoroutineRunning = false;

    private void Awake()
    {
        if (_disasters.Count == 0)
            return;

        foreach (var disaster in _disasters)
            disaster?.Init();
    }

    private void Start()
    {
        EventProvider.Subscribe<IStartFixedDisasterEvent>(StartFixedDisasterCoroutine);
        ServiceProvider.TryGetService(out _waveManager);
    }

    private void Update()
    {
        if (_disasters.Count == 0 || !_waveManager.WavesStarted)
            return;

        if (!_isCoroutineRunning)
            StartCoroutine(RandomDisasterCoroutine());

        if (_currentDisaster != null)
            (_currentDisaster as IDisasterUpdate)?.UpdateDisaster();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        _currentDisaster = null;

        EventProvider.Unsubscribe<IStartFixedDisasterEvent>(StartFixedDisasterCoroutine);
    }

    private IEnumerator RandomDisasterCoroutine()
    {
        if (_disasters.Count == 0)
            yield break;

        if (_isCoroutineRunning) yield break;
        _isCoroutineRunning = true;

        yield return new WaitForSeconds(Random.Range(_minInterval, _maxInterval));
        StartRandomDisaster();
        yield return new WaitForSeconds(_currentDisaster.Duration);
        _currentDisaster.EndDisaster();
        _isCoroutineRunning = false;
    }

    public IEnumerator DisasterCoroutine(NaturalDisaster disaster)
    {
        if (_isCoroutineRunning) yield break;
        _isCoroutineRunning = true;
        _currentDisaster = disaster;

        disaster.StartDisaster();
        yield return new WaitForSeconds(disaster.Duration);
        disaster.EndDisaster();
        _isCoroutineRunning = false;
        _currentDisaster = null;
    }


    private void StartRandomDisaster()
    {
        if (_disasters.Count == 0)
        {
            Debug.LogWarning("No disasters available to start.");
            return;
        }

        if (_currentDisaster != null)
        {
            //Debug.Log("Ending current disaster: " + _currentDisaster.name);
            _currentDisaster.EndDisaster();
        }

        if (_disasters.Count > 1)
            _currentDisaster = _disasters[Random.Range(0, _disasters.Count)];
        else
            _currentDisaster = _disasters[0];

        _currentDisaster.StartDisaster();
    }

    public void StartFixedDisasterCoroutine(IStartFixedDisasterEvent @event)
    {
        StartCoroutine(DisasterCoroutine(@event.Disaster));
    }
}
