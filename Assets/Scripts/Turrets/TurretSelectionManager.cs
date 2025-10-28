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
    [SerializeField] private int _selectedPrefab;
    [SerializeField] private float _scaleMultiplier;

    [Header("Gameplay Turrets Select")]
    [SerializeField] private TurretManager _turretManager;
    private Turret _selectedTurret;

    private void Awake()
    {
        _renderers.Clear();

        for (int i = 0; i < _turretSelectables.Count; i++)
        {
            var toAdd = _turretSelectables[i].GetComponentInChildren<SpriteRenderer>();

            if (toAdd == null)
                toAdd = _turretSelectables[i].GetComponent<SpriteRenderer>();

            _renderers.Add(toAdd);
        }

        _selectedPrefab = -1;
        _scaleMultiplier = 1.2f;

        EventProvider.Subscribe<ISelectTurretPrefabEvent>(OnSelectPrefab);
        EventProvider.Subscribe<IUpdateSelectedTurretEvent>(OnSelectTurret);
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
            if (_renderers[i] == null)
            {
                Debug.LogWarning("_renderers[i] == null");
                continue;
            }
            if (IsMouseHovering(_renderers[i].bounds))
            {
                _turretSelectables[i].gameObject.transform.localScale = new Vector3(_scaleMultiplier, _scaleMultiplier, _scaleMultiplier);
            }
            else if (i != _selectedPrefab)
                _turretSelectables[i].gameObject.transform.localScale = new Vector3(1, 1, 1);
        }

        foreach (var turret in _turretManager.ActiveTurrets)
        {
            if (!turret.SpriteRenderer)
                Debug.Log(turret.gameObject.name + ": sprite renderer not found!");

            if (IsMouseHovering(turret.SpriteRenderer.bounds))
                turret.ActivateArea();
            else
                turret.DeactivateArea();
        }
    }


    private bool IsMouseHovering(Bounds bounds)
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
        worldMousePos.z = 0;

        return bounds.Contains(worldMousePos);
    }

    public int GetSelectedTurret()
    {
        return _selectedPrefab;
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