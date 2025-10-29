using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelButtonUpdater : MonoBehaviour
{
    [SerializeField] private List<GameObject> _buttonsGOs = new();
    private int _currentlyActiveId = 0;

    private void Awake()
    {
        EventProvider.Subscribe<ILevelUpEvent>(OnLevelUp);
    }

    private void OnLevelUp(ILevelUpEvent @event)
    {
        if (_currentlyActiveId == _buttonsGOs.Count - 1)
            return;

        _buttonsGOs[_currentlyActiveId].SetActive(false);
        _currentlyActiveId++;
        _buttonsGOs[_currentlyActiveId].SetActive(true);
    }
}
