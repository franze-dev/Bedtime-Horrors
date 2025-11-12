using UnityEngine;

public interface IOnDisasterStartEvent : IEvent
{
    NaturalDisaster Disaster { get; }
}

public class OnDisasterStartEvent : IOnDisasterStartEvent
{
    private GameObject _triggeredByGO;

    public NaturalDisaster Disaster { get; private set; }
    public GameObject TriggeredByGO => _triggeredByGO;

    public OnDisasterStartEvent(NaturalDisaster disaster, GameObject gameObject = null)
    {
        Disaster = disaster;
        _triggeredByGO = gameObject;
    }
}
