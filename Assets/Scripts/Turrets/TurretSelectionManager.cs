using DragonBones;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TurretSelectionManager : MonoBehaviour
{
    [Header("Prefab select")]
    [SerializeField] private List<GameObject> _turretSelectables = new List<GameObject>();
    public List<GameObject> TurretSelectables => _turretSelectables;

    private List<SpriteRenderer> _renderers = new List<SpriteRenderer>();
    private List<BoxCollider2D> _colliders = new();

    [SerializeField] private int _selectedPrefab;
    [SerializeField] private float _scaleMultiplier;

    [Header("Gameplay Turrets Select")]
    [SerializeField] private TurretManager _turretManager;
    private Turret _selectedTurret;
    private PauseController _pauseController;
    private TutorialManager _tutorialManager;

    private void Awake()
    {
        _renderers.Clear();
        _colliders.Clear();

        foreach (var selectable in _turretSelectables)
        {
            var renderer = selectable.GetComponentInChildren<SpriteRenderer>();
            var collider = selectable.GetComponentInChildren<BoxCollider2D>();

            if (renderer != null)
            {
                _renderers.Add(renderer);
                _colliders.Add(null);
            }
            else if (collider != null)
            {
                _renderers.Add(null);
                _colliders.Add(collider);
            }
            else
            {
                _renderers.Add(null);
                _colliders.Add(null);
            }
        }

        _selectedPrefab = -1;
        _scaleMultiplier = 1.2f;

        EventProvider.Subscribe<ISelectTurretPrefabEvent>(OnSelectPrefab);
        EventProvider.Subscribe<IUpdateSelectedTurretEvent>(OnSelectTurret);
    }

    private void Start()
    {
        ServiceProvider.TryGetService(out _pauseController);
    }

    private void OnDestroy()
    {
        EventProvider.Unsubscribe<IUpdateSelectedTurretEvent>(OnSelectTurret);
        EventProvider.Unsubscribe<ISelectTurretPrefabEvent>(OnSelectPrefab);
    }

    private void OnSelectTurret(IUpdateSelectedTurretEvent @event)
    {
        if (_selectedTurret != null)
            DeselectCurrent();

        if (@event.ToSelect == null)
            DeselectCurrent();
        else if (_selectedTurret == null)
        {
            _selectedTurret = @event.ToSelect;
            _selectedTurret?.Select();
        }
        else if (@event.ToSelect == _selectedTurret)
            DeselectCurrent();
    }

    private void DeselectCurrent()
    {
        _selectedTurret?.Deselect();
        _selectedTurret = null;
    }

    private void OnSelectPrefab(ISelectTurretPrefabEvent @event)
    {
        if (_selectedPrefab != -1)
            if (@event.TurretSelectable.gameObject == _turretSelectables[_selectedPrefab])
                return;

        for (int i = 0; i < _turretSelectables.Count; i++)
        {
            var select = @event.TurretSelectable;

            if (select == _turretSelectables[i].GetComponent<TurretSelectable>())
            {
                _selectedPrefab = i;
                return;
            }
        }

        Debug.Log(@event.TurretSelectable.gameObject.name);
        _selectedPrefab = -1;
    }



    private void Update()
    {
        for (int i = 0; i < _turretSelectables.Count; i++)
        {
            Bounds bounds;

            if (_renderers[i] != null)
                bounds = _renderers[i].bounds;
            else if (_colliders[i] != null)
                bounds = _colliders[i].bounds;
            else
                continue;

            if (IsMouseHovering(bounds))
                _turretSelectables[i].transform.localScale = Vector3.one * _scaleMultiplier;
            else if (i != _selectedPrefab)
                _turretSelectables[i].transform.localScale = Vector3.one;
        }

        foreach (var turret in _turretManager.ActiveTurrets)
        {
            if (!turret.Collider)
                continue;

            if (IsMouseHovering(turret.Collider.bounds))
                turret.ActivateArea();
            else
                turret.DeactivateArea();
        }
    }


    private bool IsMouseHovering(Bounds bounds)
    {
        if (!_pauseController)
            ServiceProvider.TryGetService(out _pauseController);

        if (!_tutorialManager)
            ServiceProvider.TryGetService(out _tutorialManager);

        if ((_pauseController && _pauseController.IsPaused) ||
           (_tutorialManager && !_tutorialManager.IsClickAllowed))
            return false;

        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
        worldMousePos.z = 0;
        worldMousePos.z = bounds.center.z;

        return bounds.Contains(worldMousePos);
    }

    public int GetSelectedTurret()
    {
        return _selectedPrefab;
    }

    private void OnDrawGizmos()
    {
        if (_turretManager == null)
            return;

        Gizmos.color = Color.green;

        foreach (var turret in _turretManager.ActiveTurrets)
        {
            var bounds = turret.Collider.bounds;
            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }
    }
}

public interface IUpdateSelectedTurretEvent : IEvent
{
    Turret ToSelect { get; }
}

public class UpdateSelectedTurret : IUpdateSelectedTurretEvent
{
    private Turret _toSelect;
    public Turret ToSelect => _toSelect;
    public GameObject TriggeredByGO => null;

    public UpdateSelectedTurret(Turret toSelect)
    {
        _toSelect = toSelect;
    }
}