using UnityEngine;

public class MenuTransitionEvents : MonoBehaviour
{
    [SerializeField] private NavigationController _navigationController;

    private void Awake()
    {
        EventProvider.Subscribe<ISetAllMenusInactive>(SetAllInactive);
        EventProvider.Subscribe<IActivateTargetMenu>(ActivateTargetMenu);
    }

    private void SetAllInactive(ISetAllMenusInactive @event)
    {
        //GameEvents.TriggerSetAllMenusInactive();
        _navigationController.SetAllInactive();
        GameManager.Instance.SetState(@event.StateToTransition);
    }
    private void ActivateTargetMenu(IActivateTargetMenu @event)
    {
        if (@event.TargetMenu != null)
            _navigationController.SetMenuActive(@event.TargetMenu);
    }

}
