using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<Level> _mainLevels;
    [SerializeField] private int _startLevelId = 0;
    private int _currentLevelId = 0;
    private int LastLevelId => _mainLevels.Count - 1;

    private NavigationController _navigationController;

    private void Awake()
    {
        _currentLevelId = _startLevelId;
        ServiceProvider.SetService(this, true);
        EventProvider.Subscribe<INextLevelEvent>(OnNextLevel);
    }

    private void OnDestroy()
    {
        ServiceProvider.SetService<LevelManager>(null);
    }

    private void OnNextLevel(INextLevelEvent @event)
    {
        Level currentLevel = @event.CurrentLevel;
        int listId = GetListId(currentLevel);

        if (!IsNextLevelValid(currentLevel, listId))
        {
            _currentLevelId = _startLevelId;
            SceneController.Instance.SetLastActiveGameplay(_mainLevels[_startLevelId]);

            ServiceProvider.TryGetService(out _navigationController);
            _navigationController.GoToMenu(new MainMenuState());

            return;
        }

        Level nextLevel = _mainLevels[listId + 1];

        GoToLevel(nextLevel);
    }

    private bool IsNextLevelValid(Level currentLevel, int listId)
    {
        if (currentLevel == null || currentLevel == _mainLevels[LastLevelId] || listId == -1 || listId == LastLevelId)
            return false;

        return true;
    }

    private void GoToLevel(Level nextLevel)
    {
        SceneController.Instance.AddLevel(nextLevel);
    }

    private int GetListId(Level currentLevel)
    {
        for (int i = 0; i < _mainLevels.Count; i++)
        {
            if (currentLevel == _mainLevels[i])
                return i;
        }
        return -1;
    }

    public bool HasNextLevel(Level level)
    {
        if (level == null)
            return false;

        if (!IsMainLevel(level))
            return false;

        return level != null && level != _mainLevels[LastLevelId];
    }

    private bool IsMainLevel(Level level)
    {
        foreach (Level mainLevel in _mainLevels)
        {
            if (mainLevel == level) 
                return true;
        }

        return false;
    }

    public bool IsThereANextLevel()
    {
        return HasNextLevel(SceneController.Instance.GetLastActiveGameplay());
    }
}

public interface INextLevelEvent : IEvent
{
    Level CurrentLevel { get; }
}

public class NextLevelEvent : INextLevelEvent
{

    private Level _currentLevel;

    public Level CurrentLevel => _currentLevel;

    public GameObject TriggeredByGO => null;

    public NextLevelEvent()
    {
        _currentLevel = SceneController.Instance.GetLastActiveGameplay();
    }
}
