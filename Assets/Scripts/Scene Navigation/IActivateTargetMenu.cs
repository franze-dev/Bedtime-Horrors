using UnityEngine;

public interface IActivateTargetMenu : IEvent
{
    GameObject TargetMenu { get; }
}


public class ActivateTargetMenu : IActivateTargetMenu
{
    private GameObject _targetMenu;
    private GameObject _gameObject;

    public GameObject TargetMenu => _targetMenu;
    public GameObject TriggeredByGO => _gameObject;

    public ActivateTargetMenu(GameObject targetMenu, bool activateMenuScene = false)
    {
        this._targetMenu = targetMenu;
        _gameObject = null;

        if (activateMenuScene)
            SceneController.Instance.SetSceneActive(SceneController.Instance.levelContainer.menusLevel.scenes[0]);
    }
}