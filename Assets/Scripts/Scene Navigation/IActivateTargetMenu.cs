using UnityEngine;

public interface IActivateTargetMenu : IEvent
{
    GameObject TargetMenu { get; }
    bool DeactivatePreviousMenu { get; }
}


public class ActivateTargetMenu : IActivateTargetMenu
{
    private GameObject _targetMenu;
    private GameObject _gameObject;
    private bool _deactivatePreviousMenu;

    public GameObject TargetMenu => _targetMenu;
    public GameObject TriggeredByGO => _gameObject;
    public bool DeactivatePreviousMenu => _deactivatePreviousMenu;

    public ActivateTargetMenu(IMenuState targetMenu, bool deactivatePreviousMenu = true, bool activateMenuScene = false)
    {
        ServiceProvider.TryGetService<NavigationController>(out var controller);

        controller.GoToMenu(targetMenu, deactivatePreviousMenu);

        _gameObject = null;

        if (activateMenuScene)
            SceneController.Instance.SetSceneActive(SceneController.Instance.levelContainer.menusLevel.scenes[0]);
    }
}