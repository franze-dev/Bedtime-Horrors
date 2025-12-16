using System.Collections.Generic;
using UnityEngine;

public class LevelUpButton : MonoBehaviour, ITooltipInfo, IInteractable
{
    [SerializeField] Turret _turret;
    [SerializeField] List<GameObject> _buttonsGO;
    [SerializeField] private int _firstButtonId = 0;
    private int _currentButtonId = 0;
    private Collider2D _collider;

    public string Text => !_turret.IsOnLastLevel() ? "CREATIVITY: " + _turret.LevelUpPrice : "LEVEL MAXED OUT";

    private void Awake()
    {
        foreach (var button in _buttonsGO)
        {
            button.SetActive(false);
        }

        ActivateButton(_firstButtonId);
        _collider = GetComponent<Collider2D>();
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
        if (_turret.IsOnLastLevel())
            return;

        int levelUpPrice = (int)-_turret.LevelUpPrice;

        if (_turret.Upgrade())
        {
            ActivateButton(_currentButtonId + 1);

            EventTriggerer.Trigger<ICreativityUpdateEvent>(new CreativityUpdaterEvent(gameObject, levelUpPrice));
        }
    }

    public void Interact()
    {
        if (HoverChecker.CheckHover(_collider))
            LevelUp();
    }
}