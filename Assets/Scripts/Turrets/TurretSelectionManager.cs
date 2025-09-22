using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class TurretSelectionManager : MonoBehaviour
{
    [SerializeField] private InputActionReference _click;

    [SerializeField] private List<GameObject> _turretPrefabs = new List<GameObject>();
    private List<SpriteRenderer> _renderers = new List<SpriteRenderer>();
    private int _selectedTurret;
    [SerializeField] private float _scaleMultiplier;

    private void Awake()
    {
        _click.action.canceled += OnClick;
        _renderers.Clear();

        for (int i = 0; i < _turretPrefabs.Count; i++)
        {
            var toAdd = _turretPrefabs[i].GetComponent<SpriteRenderer>();

            if (toAdd == null)
                toAdd = _turretPrefabs[i].GetComponentInChildren<SpriteRenderer>();

            _renderers.Add(toAdd);
        }

        _selectedTurret = -1;
        _scaleMultiplier = 1.2f;
    }

    private void OnClick(InputAction.CallbackContext context)
    {
        for (int i = 0; i < _turretPrefabs.Count; i++)
        {
            if (IsMouseHovering(_renderers[i].bounds))
            {
                _selectedTurret = i;
                break;
            }
        }
    }

    private void Update()
    {
        for (int i = 0; i < _turretPrefabs.Count; i++)
        {           
            if (IsMouseHovering(_renderers[i].bounds))
            {
                Debug.Log("Mouse hovering over " + _renderers[i].gameObject.name);
                _turretPrefabs[i].gameObject.transform.localScale = new Vector3(_scaleMultiplier, _scaleMultiplier, _scaleMultiplier);
            }
            else if (i != _selectedTurret)
                _turretPrefabs[i].gameObject.transform.localScale = new Vector3(1, 1, 1);
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
        return _selectedTurret;
    }
}
