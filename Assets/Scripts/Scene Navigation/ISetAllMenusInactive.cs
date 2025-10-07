using UnityEngine;

public interface ISetAllMenusInactive : IEvent
{
    GameManager.GameState StateToTransition { get; }
}


public class SetAllMenusInactive : ISetAllMenusInactive
{
    private GameManager.GameState _stateToTransition;
    private GameObject _gameObject;


    public GameManager.GameState StateToTransition => _stateToTransition;

    public GameObject TriggeredByGO => _gameObject;

    public SetAllMenusInactive(GameManager.GameState stateToTransition)
    {
        _stateToTransition = stateToTransition;
        _gameObject = null;
    }

}
