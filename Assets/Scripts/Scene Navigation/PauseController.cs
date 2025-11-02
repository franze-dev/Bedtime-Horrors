using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    [SerializeField] private InputActionReference _pauseAction;
    [SerializeField] private NavigationController _navigationController;

    private bool isPaused = false;

    private void OnEnable()
    {
        if (_pauseAction != null)
            _pauseAction.action.performed += OnPause;

        EventProvider.Subscribe<ITogglePause>(OnTogglePauseEvent);
    }

    private void OnDisable()
    {
        if (_pauseAction != null)
            _pauseAction.action.performed -= OnPause;

        EventProvider.Unsubscribe<ITogglePause>(OnTogglePauseEvent);
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        if (GameManager.Instance == null) return;

        if (SceneController.Instance.IsGameplaySceneActive() || isPaused)
            TogglePause();
    }

    private void OnTogglePauseEvent(ITogglePause @event)
    {
        TogglePause();
    }

    public void TogglePause()
    {
        ChangePausedState();

        if (isPaused)
        {
            EventTriggerer.Trigger<IActivateTargetMenu>(new ActivateTargetMenu(new PauseMenuState(), true, true));
            GameManager.Instance.PauseTime();
        }
        else
        {
            GameManager.Instance.ResumeTime();

            _navigationController.SetAllInactive();
            SceneController.Instance.SetSceneActive(SceneController.Instance.PreviousActiveScene);
        }
    }

    private void ChangePausedState()
    {
        isPaused = !isPaused;
    }
}
