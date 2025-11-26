using DragonBones;
using Unity.VisualScripting;
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

    public override void EndDisaster()
    {
        Debug.LogWarning("End disaster entered");

        AkUnitySoundEngine.SetSwitch("Disaster_Type", "Earthquake", WwiseAudioHelper.DisasterSoundEmitter);
        AkUnitySoundEngine.PostEvent("Disaster_End", WwiseAudioHelper.DisasterSoundEmitter);

        _isRunning = false;
        originalCamPos = new(0, 0, -10);
        _camera.transform.localPosition = originalCamPos;

        DestroyRandomTurret();
        AnimationLogic?.Stop();
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
            return;
        }

        int randomIndex = Random.Range(0, activeTurrets.Count + 2);

        if (randomIndex >= activeTurrets.Count)
            return;

        GameObject turretToDestroy = activeTurrets[randomIndex].gameObject;

        if (turretToDestroy != null)
            EventTriggerer.Trigger<ITurretDestroyEvent>(new TurretDestroyEvent(turretToDestroy));

    }

    public override void Init()
    {
        Duration = _duration;

        _camera = Camera.current;
    }

    public override void StartDisaster()
    {
        AkUnitySoundEngine.SetSwitch("Disaster_Type", "Earthquake", WwiseAudioHelper.DisasterSoundEmitter);
        AkUnitySoundEngine.PostEvent("Disaster_Start", WwiseAudioHelper.DisasterSoundEmitter);

        _isRunning = true;


        Debug.LogWarning("Start earthquake");

        AnimationLogic?.Play(AnimationData);
    }

    public void UpdateDisaster()
    {
        if (_isRunning && SettingsManager.ShakeEnabled)
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

            float offsetX = Mathf.PerlinNoise(Time.time * _frequency, 0f) * 2f - 1f;
            float offsetY = Mathf.PerlinNoise(0f, Time.time * _frequency) * 2f - 1f;

            Vector3 shakePos = new Vector3(offsetX, offsetY, -10f) * _magnitude;

            _camera.transform.localPosition = originalCamPos + shakePos;
        }

        (AnimationLogic as IDisasterAnimationLoop)?.Loop();
    }
}
