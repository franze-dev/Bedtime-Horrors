using UnityEngine;

public interface IActivatePreviousMenu : IEvent
{
}


public class ActivatePreviousMenu : IActivatePreviousMenu
{
    private GameObject _gameObject;

    public GameObject TriggeredByGO => _gameObject;

    public ActivatePreviousMenu(bool activateMenuScene = false)
    {
        _gameObject = null;

        if (activateMenuScene)
            SceneController.Instance.SetSceneActive(SceneController.Instance.levelContainer.menusLevel.scenes[0]);
    }
}