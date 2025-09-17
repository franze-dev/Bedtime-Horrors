using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TurretSpawner : MonoBehaviour
{
    private List<GameObject> _turretPrefabs;

    [SerializeField] private InputActionReference _click;

    [SerializeField] private InputActionReference _spawnTurret1;
    [SerializeField] private InputActionReference _spawnTurret2;
    [SerializeField] private InputActionReference _spawnTurret3;

    [SerializeField] private CreativityUpdater _creativityUpdater;
    [SerializeField] private TurretSelectionManager _selectionManager;

    private SpriteRenderer _renderer;
    private GameObject _spawnedTurret;
    private int _nextTurretId = 0;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();

        _click.action.canceled += OnClick;
        _spawnTurret1.action.canceled += OnSpawnTurret1;
        _spawnTurret2.action.canceled += OnSpawnTurret2;
        _spawnTurret3.action.canceled += OnSpawnTurret3;

        _spawnedTurret = null;
    }

    private void OnDestroy()
    {
        _click.action.canceled -= OnClick;
    }

    private void OnClick(InputAction.CallbackContext context)
    {
        int selectedTurret = _selectionManager.GetSelectedTurret();

        if (selectedTurret >= 0 && IsMouseHovering())
            SpawnTurret(selectedTurret);
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
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector3 mouseToScreenPos = Camera.main.ScreenToWorldPoint(mousePos);
        mouseToScreenPos.z = 0;

        Bounds bounds = _renderer.sprite.bounds;
        float distance = Vector2.Distance(mouseToScreenPos, (Vector2)transform.position);

        Debug.Log("Mouse hovering spawner: " + (distance < bounds.size.x / 2));
        return (distance < bounds.size.x / 2);

    }

    private void SpawnTurret()
    {
        GameObject toDestroy = null;

        if (_spawnedTurret != null)
        {
            toDestroy = _spawnedTurret;
            _spawnedTurret = null;

            Destroy(toDestroy);

            if (_nextTurretId >= _turretPrefabs.Count - 1)
                _nextTurretId = 0;
            else
                _nextTurretId++;
        }
        _spawnedTurret = Instantiate(_turretPrefabs[_nextTurretId]);
        _spawnedTurret.transform.position = transform.position;
    }

    private void SpawnTurret(int turretId)
    {
        int turretPrice = GetTurretPrice(_turretPrefabs[turretId]);

        if (_creativityUpdater.GetCreativityValue() >= turretPrice)
        {
            GameObject toDestroy = null;

            if (_spawnedTurret != null)
            {
                toDestroy = _spawnedTurret;
                _spawnedTurret = null;

                Destroy(toDestroy);

                if (_nextTurretId >= _turretPrefabs.Count - 1)
                    _nextTurretId = 0;
                else
                    _nextTurretId++;
            }
            _spawnedTurret = Instantiate(_turretPrefabs[turretId]);
            _spawnedTurret.transform.position = transform.position;

            EventTriggerer.Trigger<ICreativityUpdateEvent>(new CreativityUpdaterEvent(gameObject, -turretPrice));
        }

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
        if (turretGO.gameObject.TryGetComponent<Turret>(out Turret turret))
        {
            return turret.price;
        }
        else return 0;
    }
}