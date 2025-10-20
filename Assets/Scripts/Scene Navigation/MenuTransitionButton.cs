using UnityEngine;
using UnityEngine.UI;

public class MenuTransitionButton : MonoBehaviour
{
    [SerializeField] private GameObject _targetMenu;
    [SerializeField] private NavigationController _navigationController;

    public void ActivateBaseMenu()
    {
        //GameEvents.TriggerActivateBaseMenu();
        _navigationController.SetMenuActive(_navigationController.mainMenuGO);
    }

    /// <summary>
    /// Triggers an event that sets the targetMenu as active
    /// and the current state as the stateToTransition
    /// </summary>
    public void ButtonSetTargetActive()
    {
        EventTriggerer.Trigger<IActivateTargetMenu>(new ActivateTargetMenu(_targetMenu));
    }

    /// <summary>
    /// Calls the SetAllInactive() function from NavigationController.
    /// Sets the GameManager state to the assigned target state
    /// </summary>
    public void ButtonSetAllInactive()
    {
        EventTriggerer.Trigger<ISetAllMenusInactive>(new SetAllMenusInactive());
        Debug.Log("All menus inactive Event triggered!");
    }

    public void ButtonTogglePause()
    {
        EventTriggerer.Trigger<ITogglePause>(new TogglePauseEvent());
    }

    public void ExitApp()
    {
        Application.Quit();
    }
}
