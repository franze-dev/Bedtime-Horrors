using UnityEngine;

public interface ISetAllMenusInactive : IEvent
{
}


public class SetAllMenusInactive : ISetAllMenusInactive
{
    private GameObject _gameObject;

    public GameObject TriggeredByGO => _gameObject;

    public SetAllMenusInactive()
    {
        _gameObject = null;
    }

}
