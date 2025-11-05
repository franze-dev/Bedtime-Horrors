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
    }

    private void OnNextLevel(INextLevelEvent @event)
    {
        if (!_levelManager.IsThereANextLevel())
            _nextLevelButton.gameObject.SetActive(false);
        else
            _nextLevelButton.gameObject.SetActive(true);
    }
}