using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpButton : MonoBehaviour
{
    [SerializeField] Turret _turret;
    [SerializeField] List<GameObject> _buttonsGO;
    [SerializeField] private int _firstButtonId = 0;
    private int _currentButtonId = 0;

    private void Awake()
    {
        foreach (var button in _buttonsGO)
        {
            button.SetActive(false);
        }

        ActivateButton(_firstButtonId);
    }

    private void ActivateButton(int id)
    {
        if (id >= _buttonsGO.Count)
            return;

        if (_currentButtonId < _buttonsGO.Count)
            _buttonsGO[_currentButtonId].SetActive(false);

        _currentButtonId = id;

        _buttonsGO[_currentButtonId].SetActive(true);
    }

    public void LevelUp()
    {
        if (_turret.Upgrade())
        {
            ActivateButton(_currentButtonId + 1);

            EventTriggerer.Trigger<ICreativityUpdateEvent>(new CreativityUpdaterEvent(gameObject, (int)-_turret.LevelUpPrice));
            EventTriggerer.Trigger<ILevelUpEvent>(new LevelUpEvent());
        }
    }

}

public interface ILevelUpEvent : IEvent
{

}

public class LevelUpEvent : ILevelUpEvent
{
    public GameObject TriggeredByGO => null;
}