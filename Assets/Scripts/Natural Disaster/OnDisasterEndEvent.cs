using UnityEngine;

public interface IOnDisasterEndEvent : IEvent
{
    NaturalDisaster Disaster { get; }
}

public class OnDisasterEndEvent : IOnDisasterEndEvent
{
    private GameObject _triggeredByGO;

    public NaturalDisaster Disaster { get; private set; }
    public GameObject TriggeredByGO => _triggeredByGO;

    public OnDisasterEndEvent(NaturalDisaster disaster, GameObject gameObject = null)
    {
        Disaster = disaster;
        _triggeredByGO = gameObject;
    }
}
