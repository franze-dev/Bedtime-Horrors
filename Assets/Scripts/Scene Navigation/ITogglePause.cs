using UnityEngine;

public interface ITogglePause : IEvent { }

public class TogglePauseEvent : ITogglePause
{
    private GameObject _gameObject;

    public GameObject TriggeredByGO => _gameObject;

    public TogglePauseEvent()
    {
        _gameObject = null;
    }
}
