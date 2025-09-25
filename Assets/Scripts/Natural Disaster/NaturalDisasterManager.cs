using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaturalDisasterManager : MonoBehaviour
{
    [SerializeField] private List<NaturalDisaster> _disasters;
    [SerializeField] private float _minInterval = 30f;
    [SerializeField] private float _maxInterval = 120f;
    private NaturalDisaster _currentDisaster;

    bool _isCoroutineRunning = false;

    private void Awake()
    {
        foreach (var disaster in _disasters)
            disaster?.Init();

        StartCoroutine(DisasterCoroutine());
    }

    private void Update()
    {
        if (!_isCoroutineRunning)
            StartCoroutine(DisasterCoroutine());

        if (_currentDisaster != null)
            (_currentDisaster as IDisasterUpdate)?.UpdateDisaster();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private IEnumerator DisasterCoroutine()
    {
        if (_isCoroutineRunning) yield break;
        _isCoroutineRunning = true;
        yield return new WaitForSeconds(Random.Range(_minInterval, _maxInterval));
        StartRandomDisaster();
        yield return new WaitForSeconds(_currentDisaster.Duration);
        _currentDisaster.EndDisaster();
        _isCoroutineRunning = false;
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
            Debug.Log("Ending current disaster: " + _currentDisaster.name);
            _currentDisaster.EndDisaster();
        }

        if (_disasters.Count > 1)
            _currentDisaster = _disasters[Random.Range(0, _disasters.Count)];
        else
            _currentDisaster = _disasters[0];

        _currentDisaster.StartDisaster();
    }
}
