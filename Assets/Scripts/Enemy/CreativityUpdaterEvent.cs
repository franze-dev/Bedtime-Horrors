using UnityEngine;

public class CreativityUpdaterEvent : ICreativityUpdateEvent
{
    private int _value;
    private GameObject _triggeredByGO;

    public int Value => _value;

    public GameObject TriggeredByGO => _triggeredByGO;

    public CreativityUpdaterEvent(GameObject gameObject, int value)
    {
        _value = value;
        _triggeredByGO = gameObject;
    }
}