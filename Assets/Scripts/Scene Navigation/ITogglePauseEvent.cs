using UnityEngine;

public interface ITogglePauseEvent : IEvent { }

public class TogglePauseEvent : ITogglePauseEvent
{
    private GameObject _gameObject;
    public GameObject TriggeredByGO => _gameObject;

    public TogglePauseEvent()
    {
        _gameObject = null;
    }
}
