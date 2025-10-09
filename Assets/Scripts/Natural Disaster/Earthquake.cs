using DragonBones;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "RandomTowerDestruction", menuName = "ScriptableObjects/NaturalDisasters/RandomTowerDestruction")]
public class Earthquake : NaturalDisaster, IDisasterUpdate
{
    [SerializeField] private float _duration = 3f;
    [SerializeField] private DisasterAnimation _disasterAnimation;
    [Header("Shake Parameters")]
    [SerializeField] private float _magnitude = 0.2f;
    [SerializeField] private float _frequency = 25f;

    private Vector3 originalCamPos;
    private Camera _camera = null;
    private bool _isRunning = false;
    private GameObject animationObject;

    public override void EndDisaster()
    {
        _isRunning = false;
        originalCamPos = new(0, 0, -10);
        _camera.transform.localPosition = originalCamPos;

        DestroyRandomTurret();
        EndAnimation();
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

        GameObject turretToDestroy = activeTurrets[randomIndex].gameObject;

        if (turretToDestroy != null)
        {
            EventTriggerer.Trigger<ITurretDestroyEvent>(new TurretDestroyEvent(turretToDestroy));
            EventTriggerer.Trigger<ILogMessageEvent>(new LogMessageEvent("A turret was destroyed..", null));
        }
    }

    public override void Init()
    {
        Duration = _duration;
        DisasterAnimation = _disasterAnimation;

        _camera = Camera.current;
    }

    public override void StartDisaster()
    {
        _isRunning = true;
        EventTriggerer.Trigger<ILogMessageEvent>(new LogMessageEvent("Earthquake!", null));

       StartAnimation();
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

            float offsetX = Mathf.PerlinNoise(Time.time * _frequency, 0f) * 2f - 1f;
            float offsetY = Mathf.PerlinNoise(0f, Time.time * _frequency) * 2f - 1f;

            Vector3 shakePos = new Vector3(offsetX, offsetY, -10f) * _magnitude;

            _camera.transform.localPosition = originalCamPos + shakePos;
        }
    }

    public override void StartAnimation()
    {
        animationObject = DisasterAnimation.animationPrefab;
        Instantiate(animationObject);
        var animationArmature = animationObject.GetComponent<UnityArmatureComponent>();
        animationArmature.animation.Play(animationArmature.animation.animationNames[0]);
    }

    public override void EndAnimation()
    {
        Destroy(animationObject);
    }
}
