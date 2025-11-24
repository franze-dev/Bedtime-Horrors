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
        if (!_levelManager.IsThereANextLevelCurr())
            _nextLevelButton.gameObject.SetActive(false);
        else
            _nextLevelButton.gameObject.SetActive(true);
    }

    private void OnNextLevel(INextLevelEvent @event)
    {
        if (!_levelManager.IsThereANextLevelPrev())
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
