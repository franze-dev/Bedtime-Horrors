using UnityEngine;

public class MenuTransitionEvents : MonoBehaviour
{
    [SerializeField] private NavigationController _navigationController;

    private void Awake()
    {
        EventProvider.Subscribe<ISetAllMenusInactive>(SetAllInactive);
        //EventProvider.Subscribe<IActivateTargetMenu>(ActivateTargetMenu);
        //EventProvider.Subscribe<IActivatePreviousMenu>(ActivatePreviousMenu);
    }

    private void OnDestroy()
    {
        EventProvider.Unsubscribe<ISetAllMenusInactive>(SetAllInactive);
    }

    private void SetAllInactive(ISetAllMenusInactive @event)
    {
        _navigationController.SetAllInactive();
    }
    //private void ActivateTargetMenu(IActivateTargetMenu @event)
    //{
    //    if (@event.TargetMenu != null)
    //        _navigationController.SetMenuActive(@event.TargetMenu, @event.DeactivatePreviousMenu);
    //}

    //private void ActivatePreviousMenu(IActivatePreviousMenu @event)
    //{
    //    _navigationController.SetMenuActive(_navigationController.PreviousMenu);
    //}

}
