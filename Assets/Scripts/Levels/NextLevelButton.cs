using System;
using UnityEngine;
using UnityEngine.UI;

public class NextLevelButton : MonoBehaviour
{
    [SerializeField] private Button _nextLevelButton;
    private LevelManager _levelManager;

    private void Start()
    {
        ServiceProvider.TryGetService(out _levelManager);
        EventProvider.Subscribe<INextLevelEvent>(OnNextLevel);
        EventProvider.Subscribe<ICheckNextLevelEvent>(OnCheckNextLevel);
    }

    private void OnCheckNextLevel(ICheckNextLevelEvent @event)
    {
        CheckNextLevel();
    }

    private void OnNextLevel(INextLevelEvent @event)
    {
        CheckNextLevel();
    }

    private void CheckNextLevel()
    {
        if (!_levelManager.IsThereANextLevel())
            _nextLevelButton.gameObject.SetActive(false);
        else
            _nextLevelButton.gameObject.SetActive(true);
    }
}

public interface ICheckNextLevelEvent : IEvent
{
}

public class CheckNextLevelEvent : ICheckNextLevelEvent
{
    public GameObject TriggeredByGO => null;
}
