using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelScreen : MonoBehaviour
{
    [SerializeField] private List<Button> _levelButtons;
    private LevelManager _levelManager;

    private void Awake()
    {
        for (int i = 1; i < _levelButtons.Count; i++)
            _levelButtons[i].interactable = false;
    }

    private void Start()
    {
        EventProvider.Subscribe<INextLevelEvent>(OnNextLevel);
        ServiceProvider.TryGetService(out _levelManager);
    }

    private void OnNextLevel(INextLevelEvent @event)
    {
        var nextLevel = _levelManager.GetNextLevel(@event.CurrentLevel);
        var listId = _levelManager.GetListId(nextLevel);

        _levelButtons[listId].interactable = true;
    }
}
