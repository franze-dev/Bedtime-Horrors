using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TurretSpawner : MonoBehaviour, IInteractable
{
    private List<GameObject> _turretPrefabs;

    [SerializeField] private InputActionReference _turretDeleteInput;

    [SerializeField] private InputActionReference _spawnTurret1;
    [SerializeField] private InputActionReference _spawnTurret2;
    [SerializeField] private InputActionReference _spawnTurret3;

    [SerializeField] private TurretSelectionManager _selectionManager;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private GameObject _giftBoxGO;

    [SerializeField] private float _cooldown = 1;
    private float _currentTime = 0;

    private GameObject _spawnedTurret;
    private int _nextTurretId = 0;
    private CreativityUpdater _creativityUpdater;
    private PauseController _pauseController;
    private TutorialManager _tutorialManager;

    private void Awake()
    {
        if (_giftBoxGO == null)
            Debug.LogError("giftboxGO not found in " + gameObject.name);

        if (_renderer == null)
            _renderer = _giftBoxGO.GetComponent<SpriteRenderer>();

        _turretDeleteInput.action.canceled += OnDeletion;
        _spawnTurret1.action.canceled += OnSpawnTurret1;
        _spawnTurret2.action.canceled += OnSpawnTurret2;
        _spawnTurret3.action.canceled += OnSpawnTurret3;

        _spawnedTurret = null;
    }

    private void Start()
    {
        ServiceProvider.TryGetService(out _creativityUpdater);
        ServiceProvider.TryGetService(out _pauseController);
        ServiceProvider.TryGetService(out _tutorialManager);
    }

    private void OnDeletion(InputAction.CallbackContext context)
    {
        if (IsMouseHovering())
            DeleteTurret();
    }

    private void DeleteTurret()
    {
        if (_spawnedTurret == null)
            return;

        Turret turret = _spawnedTurret.GetComponent<Turret>();

        EventTriggerer.Trigger<ICreativityUpdateEvent>(new CreativityUpdaterEvent(gameObject, turret.price));

        EventTriggerer.Trigger<ITurretDestroyEvent>(new TurretDestroyEvent(_spawnedTurret));
    }

    private void Update()
    {
        if (_spawnedTurret == null && !_giftBoxGO.activeSelf)
            _giftBoxGO.SetActive(true);

        _currentTime += Time.deltaTime;
    }

    private void OnDestroy()
    {
        _spawnTurret1.action.canceled -= OnSpawnTurret1;
        _spawnTurret2.action.canceled -= OnSpawnTurret2;
        _spawnTurret3.action.canceled -= OnSpawnTurret3;
    }

    private void OnSpawnTurret1(InputAction.CallbackContext context)
    {
        if (IsMouseHovering())
            SpawnTurret(0);
    }

    private void OnSpawnTurret2(InputAction.CallbackContext context)
    {
        if (IsMouseHovering())
            SpawnTurret(1);
    }

    private void OnSpawnTurret3(InputAction.CallbackContext context)
    {
        if (IsMouseHovering())
            SpawnTurret(2);
    }

    private bool IsMouseHovering()
    {
        if (!_pauseController)
            ServiceProvider.TryGetService(out _pauseController);

        if (!_tutorialManager)
            ServiceProvider.TryGetService(out _tutorialManager);

        if ((_pauseController && _pauseController.IsPaused) ||
           (_tutorialManager && !_tutorialManager.IsClickAllowed))
            return false;

        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector3 mouseToScreenPos = Camera.main.ScreenToWorldPoint(mousePos);
        mouseToScreenPos.z = 0;

        Bounds bounds = _renderer.bounds;

        bool contained = bounds.Contains(mouseToScreenPos);
        return contained;
    }

    private void SpawnTurret(int turretId)
    {
        if (!SceneController.Instance.IsGameplaySceneActive())
            return;

        if (_spawnedTurret != null)
            if (_spawnedTurret.GetType() == _turretPrefabs[turretId].GetType())
                return;

        if (_currentTime < _cooldown)
        {
            Debug.Log("Time: " + _currentTime);
            return;
        }

        int turretPrice = GetTurretPrice(_turretPrefabs[turretId]);

        if (_creativityUpdater.GetCreativityValue() < turretPrice)
            return;

        GameObject toDestroy = null;

        if (_spawnedTurret != null)
        {
            toDestroy = _spawnedTurret;
            _spawnedTurret = null;

            EventTriggerer.Trigger<ITurretDestroyEvent>(new TurretDestroyEvent(toDestroy));

            if (_nextTurretId >= _turretPrefabs.Count - 1)
                _nextTurretId = 0;
            else
                _nextTurretId++;
        }
        _spawnedTurret = Instantiate(_turretPrefabs[turretId], _selectionManager.TurretInstanceParent.transform);
        _spawnedTurret.transform.position = transform.position;
        _giftBoxGO.SetActive(false);

        _currentTime = 0;

        EventTriggerer.Trigger<IDeselectTurretPrefabEvent>(new DeselectTurretPrefabEvent());
        EventTriggerer.Trigger<ICreativityUpdateEvent>(new CreativityUpdaterEvent(gameObject, -turretPrice));
    }

    public void SetTurretPrefabs(List<GameObject> turretPrefabs)
    {
        if (_turretPrefabs == null)
            _turretPrefabs = new List<GameObject>();

        foreach (var prefab in turretPrefabs)
        {
            if (_turretPrefabs.Find(x => x.name == prefab.name) != null)
                throw new Exception("Duplicate turret prefab names are not allowed");

            _turretPrefabs.Add(prefab);
        }
    }


    private int GetTurretPrice(GameObject turretGO)
    {
        if (turretGO.gameObject.TryGetComponent(out Turret turret))
        {
            return turret.price;
        }
        else return 0;
    }

    public void Interact()
    {
        int selectedTurret = _selectionManager.GetSelectedTurret();

        if (selectedTurret >= 0)
            SpawnTurret(selectedTurret);
    }
}

public interface IFirstTurretSpawnEvent : IEvent
{
}

public class FirstTurretSpawnEvent : IFirstTurretSpawnEvent
{
    public GameObject TriggeredByGO => null;

    public FirstTurretSpawnEvent()
    {
        EventTriggerer.Trigger<IContinuePanelsEvent>(new ContinuePanelsEvent());
    }
}