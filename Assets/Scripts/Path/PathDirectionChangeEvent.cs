using UnityEngine;

internal class PathDirectionChangeEvent : IPathDirectionChangeEvent
{
    private GameObject _gameObject;
    public GameObject TriggeredByGO => _gameObject;

    public PathDirectionChangeEvent(GameObject gameObject)
    {
        this._gameObject = gameObject;
    }

}