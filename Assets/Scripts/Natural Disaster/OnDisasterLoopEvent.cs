using UnityEngine;

public interface IOnDisasterLoopEvent : IEvent
{
    NaturalDisaster Disaster { get; }
}

public class OnDisasterLoopEvent : IOnDisasterLoopEvent
{
    private GameObject _triggeredByGO;

    public NaturalDisaster Disaster { get; private set; }
    public GameObject TriggeredByGO => _triggeredByGO;

    public OnDisasterLoopEvent(NaturalDisaster disaster, GameObject gameObject = null)
    {
        Disaster = disaster;
        _triggeredByGO = gameObject;
    }
}
