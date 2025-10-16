using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private List<Wave> _waves;
    private int _currentWaveIndex;
    private float _timer;

    private void Awake()
    {
        _currentWaveIndex = 0;
        _timer = 0f;

        for (int i = 0; i < _waves.Count; i++)
        {
            _waves[i].InitWave();
        }
    }

    private void Update()
    {
        _timer += Time.deltaTime;


        if (_currentWaveIndex >= _waves.Count)
        {
            _timer = 0f;
            //_currentWaveIndex = 0;
            //finish level
            return;
        }
        else if (!_waves[_currentWaveIndex].IsWaveOver())
        {
            var currentWave = _waves[_currentWaveIndex];
            if (_timer >= currentWave.cooldownBetweenEnemies)
            {
                currentWave.SpawnEnemy();
                _timer = 0;
            }
        }
        else if (_currentWaveIndex < _waves.Count && _timer >= _waves[_currentWaveIndex].timeToNextWave)
        {
            _timer = 0;
            _currentWaveIndex++;
            Debug.Log("Wave changed to wave: " + (_currentWaveIndex + 1));
        }
    }
}
