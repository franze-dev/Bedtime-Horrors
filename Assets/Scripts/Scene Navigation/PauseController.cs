using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    [SerializeField] private Menu _pauseMenu;
    [SerializeField] private InputActionReference _pauseAction;
    [SerializeField] private NavigationController _navigationController;

    private bool isPaused = false;

    private void OnEnable()
    {
        if (_pauseAction != null)
        {
            _pauseAction.action.performed += OnPause;
        }
    }

    private void OnDisable()
    {
        if (_pauseAction != null)
        {
            _pauseAction.action.performed -= OnPause;
        }
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        if (GameManager.Instance == null)
            return;

        var currentState = GameManager.Instance.CurrentState;

        if (currentState == GameManager.GameState.Gameplay || currentState == GameManager.GameState.Paused)
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        ChangePausedState();

        if (isPaused)
        {
            GameManager.Instance.PauseTime();
            GameManager.Instance.SetState(GameManager.GameState.Paused);

            _navigationController.SetMenuActive(_pauseMenu);
            int pauseSceneBuildIndex = _pauseMenu.gameObject.scene.buildIndex;
            SceneController.Instance.SetSceneActive(pauseSceneBuildIndex);
        }
        else
        {
            GameManager.Instance.ResumeTime();
            GameManager.Instance.SetState(GameManager.GameState.Gameplay);

            _navigationController.SetAllInactive();
            SceneController.Instance.SetSceneActive(SceneController.Instance.PreviousActiveScene.Index);
        }
    }

    public void ChangePausedState()
    {
        isPaused = !isPaused;
    }
}
