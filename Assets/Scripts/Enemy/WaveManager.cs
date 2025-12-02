using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private List<Wave> _waves;
    private int _currentWaveIndex;
    private float _timer;
    [SerializeField] private bool _wavesStarted;

    public int WavesCount => _waves.Count;

    public bool WavesStarted => _wavesStarted;

    private void Awake()
    {
        _currentWaveIndex = 0;
        _timer = 0f;

        ServiceProvider.SetService(this, true);
    }

    private void Start()
    {
        for (int i = 0; i < _waves.Count; i++)
            _waves[i].InitWave();
    }

    private void OnDestroy()
    {
        ServiceProvider.SetService<WaveManager>(null);
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if (!_wavesStarted)
            return;

        if (_currentWaveIndex < _waves.Count)
        {
            if (!_waves[_currentWaveIndex].IsWaveOver())
            {
                var currentWave = _waves[_currentWaveIndex];
                if (_timer >= currentWave.cooldownBetweenEnemies)
                {
                    currentWave.SpawnEnemy();
                    _timer = 0;
                }
            }
            else if (_currentWaveIndex < _waves.Count) // a wave ends
            {
                if (_timer >= _waves[_currentWaveIndex].timeToNextWave) // next wave starts
                {
                    if (_waves[_currentWaveIndex].Disaster != null)
                        EventTriggerer.Trigger<IStartFixedDisasterEvent>(new StartFixedDisasterEvent(_waves[_currentWaveIndex].Disaster));

                    _timer = 0;
                    _currentWaveIndex++;

                    EventTriggerer.Trigger<INewWaveEvent>(new NewWaveEvent());
                }
            }
        }
        else if (_waves[_waves.Count - 1].AreEnemiesDead())
        {
            Debug.Log("All waves completed!");

            if (ServiceProvider.TryGetService(out NavigationController nav))
            {
                GlobalAudioEventsCaller.StopGameplaySounds();
                nav.GoToMenu(new WinMenuState());
            }
            else
                Debug.LogWarning("NavigationController service not found!");
            return;
        }
    }

    public void StartWaves()
    {
        _wavesStarted = true;
    }
}

public class NewWaveEvent : INewWaveEvent
{
    public GameObject TriggeredByGO => null;
}

public interface INewWaveEvent : IEvent
{
}

public interface IStartFixedDisasterEvent : IEvent
{
    NaturalDisaster Disaster { get; }
}

public class StartFixedDisasterEvent : IStartFixedDisasterEvent
{
    private NaturalDisaster _disaster;

    public NaturalDisaster Disaster => _disaster;
    public GameObject TriggeredByGO => null;

    public StartFixedDisasterEvent(NaturalDisaster disaster)
    {
        _disaster = disaster;
        EventTriggerer.Trigger<IContinuePanelsEvent>(new ContinuePanelsEvent());
    }
}