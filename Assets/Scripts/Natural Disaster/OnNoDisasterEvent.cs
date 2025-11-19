using UnityEngine;

public interface IOnNoDisasterEvent : IEvent { }

public class OnNoDisasterEvent : IOnNoDisasterEvent
{
    public GameObject TriggeredByGO { get; }

    public OnNoDisasterEvent(GameObject go = null)
    {
        TriggeredByGO = go;
    }
}
