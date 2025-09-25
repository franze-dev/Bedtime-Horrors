using UnityEngine;
using UnityEngine.UI;

public class MenuTransitionButton : MonoBehaviour
{
    [SerializeField] private Menu _targetMenu;
    [SerializeField] private NavigationController _navigationController;
    [SerializeField] private GameManager.GameState _stateToTransition;

    public void ActivateBaseMenu()
    {
        //GameEvents.TriggerActivateBaseMenu();
        _navigationController.SetMenuActive(_navigationController.baseMenu);
        GameManager.Instance.SetState(GameManager.GameState.MainMenu);
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
        EventTriggerer.Trigger<ISetAllMenusInactive>(new SetAllMenusInactive(_stateToTransition));
        Debug.Log("All menus inactive Event triggered!");
    }
}
