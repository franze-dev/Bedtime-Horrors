using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Level firstLevel;
    private SpeedButton _speedButton;
    private TutorialManager _tutorialManager;

    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        SceneController.Instance.LoadLevel(firstLevel);
        ServiceProvider.TryGetService(out _tutorialManager);
    }

    /// <summary>
    /// Destroys the instance of the player and unloads all scenes.
    /// Then loads the first scene again
    /// </summary>
    public void ResetGame()
    {
        SceneController.Instance.UnloadNonPersistentScenes();
        SceneController.Instance.LoadLevel(firstLevel);
    }

    /// <summary>
    /// Pauses the time by setting the timeScale to 0
    /// </summary>
    public void PauseTime()
    {
        Time.timeScale = 0f;
    }

    /// <summary>
    /// Resumes the time by setting the timeScale to 1
    /// </summary>
    public void ResumeTime()
    {
        Time.timeScale = 1f;
    }

    public void ResumeGameTime()
    {
        if (_speedButton == null)
            ServiceProvider.TryGetService(out _speedButton);

        if (!_tutorialManager)
            ServiceProvider.TryGetService(out _tutorialManager);

        if (_tutorialManager && _tutorialManager.IsTutorialRunning)
        {
            return;
        }

        Time.timeScale = _speedButton.CurrentSpeed;
    }

}
