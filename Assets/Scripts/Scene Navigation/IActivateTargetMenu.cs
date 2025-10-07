using UnityEngine;

public interface IActivateTargetMenu : IEvent
{
    Menu TargetMenu { get; }
}


public class ActivateTargetMenu : IActivateTargetMenu
{
    private Menu _targetMenu;
    private GameObject _gameObject;

    public Menu TargetMenu => _targetMenu;
    public GameObject TriggeredByGO => _gameObject;

    public ActivateTargetMenu(Menu targetMenu)
    {
        this._targetMenu = targetMenu;
        _gameObject = null;
    }
}