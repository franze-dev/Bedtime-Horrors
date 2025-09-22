using UnityEngine;

[CreateAssetMenu(fileName = "RandomTowerDestruction", menuName = "ScriptableObjects/NaturalDisasters/RandomTowerDestruction")]
public class Earthquake : NaturalDisaster, IDisasterUpdate
{
    [SerializeField] private float _duration = 3f;
    [Header("Shake Parameters")]
    [SerializeField] private float _magnitude = 0.2f;
    [SerializeField] private float _frequency = 25f;

    private Vector3 originalCamPos;
    private Camera _camera = null;
    private bool _isRunning = false;
    private float _elapsedTime = 0f;

    public override void EndDisaster()
    {
        _isRunning = false;
        originalCamPos = new(0, 0, -10);
        _camera.transform.localPosition = originalCamPos;

        DestroyRandomTurret();
    }

    private void DestroyRandomTurret()
    {
        TurretManager turretManager = FindAnyObjectByType<TurretManager>();

        if (turretManager == null)
        {
            Debug.LogError("TurretManager not found!");
            return;
        }

        var activeTurrets = turretManager.ActiveTurrets;

        if (activeTurrets == null || activeTurrets.Count == 0)
        {
            Debug.LogWarning("No active turrets to destroy.");
            EventTriggerer.Trigger<ILogMessageEvent>(new LogMessageEvent("No turrets to destroy!", null));
            return;
        }

        int randomIndex = Random.Range(0, activeTurrets.Count + 2);

        if (randomIndex >= activeTurrets.Count)
        {
            EventTriggerer.Trigger<ILogMessageEvent>(new LogMessageEvent("No casualties!", null));
            return;
        }

        GameObject turretToDestroy = activeTurrets[randomIndex];

        if (turretToDestroy != null)
        {
            EventTriggerer.Trigger<ITurretDestroyEvent>(new TurretDestroyEvent(turretToDestroy));
            EventTriggerer.Trigger<ILogMessageEvent>(new LogMessageEvent("A turret was destroyed..", null));
        }
    }

    public override void Init()
    {
        Duration = _duration;

        _camera = Camera.current;
    }

    public override void StartDisaster()
    {
        _isRunning = true;
        EventTriggerer.Trigger<ILogMessageEvent>(new LogMessageEvent("Earthquake!", null));
    }

    public void UpdateDisaster()
    {
        if (_isRunning)
        {
            if (_camera == null)
                _camera = Camera.main;

            if (_camera == null)
                _camera = Camera.current;

            if (_camera == null)
            {
                Debug.LogError("Main Camera not found!");
                return;
            }

            _elapsedTime += Time.deltaTime;

            float offsetX = Mathf.PerlinNoise(Time.time * _frequency, 0f) * 2f - 1f;
            float offsetY = Mathf.PerlinNoise(0f, Time.time * _frequency) * 2f - 1f;

            Vector3 shakePos = new Vector3(offsetX, offsetY, -10f) * _magnitude;

            _camera.transform.localPosition = originalCamPos + shakePos;
        }
    }
}
