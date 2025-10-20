using UnityEngine;

public class TurretSelectable : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        EventTriggerer.Trigger<ISelectTurretPrefabEvent>(new SelectTurretPrefabEvent(this));
    }
}

public class SelectTurretPrefabEvent : ISelectTurretPrefabEvent
{
    private TurretSelectable _turretSelectable;
    public GameObject TriggeredByGO => null;

    public TurretSelectable TurretSelectable => _turretSelectable;

    public SelectTurretPrefabEvent(TurretSelectable turretSelectable)
    {
        this._turretSelectable = turretSelectable;
    }
}

public interface ISelectTurretPrefabEvent : IEvent
{
    TurretSelectable TurretSelectable { get;}
}