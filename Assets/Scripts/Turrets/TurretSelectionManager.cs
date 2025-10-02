using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TurretSelectionManager : MonoBehaviour
{
    [SerializeField] private InputActionReference _click;

    [Header("Prefab select")]
    [SerializeField] private List<GameObject> _turretPrefabs = new List<GameObject>();
    private List<SpriteRenderer> _renderers = new List<SpriteRenderer>();
    [SerializeField] private int _selectedPrefab;
    [SerializeField] private float _scaleMultiplier;

    [Header("Gameplay Turrets Select")]
    [SerializeField] private TurretManager _turretManager;
    private Turret _selectedTurret;

    private void Awake()
    {
        _click.action.canceled += OnClick;
        _renderers.Clear();

        for (int i = 0; i < _turretPrefabs.Count; i++)
        {
            var toAdd = _turretPrefabs[i].GetComponentInChildren<SpriteRenderer>();

            if (toAdd == null)
                toAdd = _turretPrefabs[i].GetComponent<SpriteRenderer>();

            _renderers.Add(toAdd);
        }

        _selectedPrefab = -1;
        _scaleMultiplier = 1.2f;
    }

    private void OnDestroy()
    {
        _click.action.canceled -= OnClick;
    }

    private void OnClick(InputAction.CallbackContext context)
    {   
        if (_selectedTurret != null)
        {
            _selectedTurret.Deselect();
            _selectedTurret = null;
        }

        for (int i = 0; i < _turretPrefabs.Count; i++)
        {
            if (IsMouseHovering(_renderers[i].bounds))
            {
                _selectedPrefab = i;
                return;
            }
        }

        _selectedPrefab = -1;

        foreach (var turret in _turretManager.ActiveTurrets)
        {
            if (turret == null)
                continue;

            if (IsMouseHovering(turret.spriteRenderer.bounds))
            {
                if (_selectedTurret != null)
                    _selectedTurret.Deselect();

                var previous = _selectedTurret;

                _selectedTurret = turret;

                if (previous != _selectedTurret)
                    _selectedTurret.Select();
                else
                {
                    _selectedTurret.Deselect();
                    _selectedTurret = null;
                }
            }
        }    
    }

    private void Update()
    {
        for (int i = 0; i < _turretPrefabs.Count; i++)
        {
            if (_renderers[i] == null)
            {
                Debug.LogWarning("_renderers[i] == null");
                continue;
            }
            if (IsMouseHovering(_renderers[i].bounds))
            {
                Debug.Log("Mouse hovering over " + _renderers[i].gameObject.name);
                _turretPrefabs[i].gameObject.transform.localScale = new Vector3(_scaleMultiplier, _scaleMultiplier, _scaleMultiplier);
            }
            else if (i != _selectedPrefab)
                _turretPrefabs[i].gameObject.transform.localScale = new Vector3(1, 1, 1);
        }

        foreach (var turret in _turretManager.ActiveTurrets)
        {
            if (!turret.spriteRenderer)
                Debug.Log(turret.gameObject.name + ": sprite renderer not found!");

            if (IsMouseHovering(turret.spriteRenderer.bounds))
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
