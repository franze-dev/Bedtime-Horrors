using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestController : MonoBehaviour
{
    [SerializeField] private Button _chestButton;
    [SerializeField] private GameObject _chestBar;
    [SerializeField] private Sprite _closedChestSprite;
    [SerializeField] private Sprite _openChestSprite;

    [SerializeField] private bool _chestStartStatus;

    [SerializeField] private TurretSelectionManager _turretSelectionManager;
    private List<GameObject> _turretSelectables;

    private bool _isOpen;

    private void Awake()
    {
        _isOpen = _chestStartStatus;
        _turretSelectables = _turretSelectionManager.TurretSelectables;
        SetBarActive(_isOpen);
    }

    public void ToggleChest()
    {
        _isOpen = !_isOpen;
        SetBarActive(_isOpen);
        UpdateChestVisual();
    }

    private void SetBarActive(bool active)
    {
        if (_chestBar != null)
            _chestBar.SetActive(active);

        foreach (var turretPrefab in _turretSelectables)
        {
            if (turretPrefab != null)
                turretPrefab.SetActive(active);
        }
    }

    private void UpdateChestVisual()
    {
        _chestButton.image.sprite = _isOpen ? _openChestSprite : _closedChestSprite;
    }
}
