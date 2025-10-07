using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState 
    { 
        Boot,
        MainMenu,
        Credits,
        Gameplay,
        Paused,
        Victory
    }
    public GameState CurrentState { get; private set; }

    [SerializeField] private Level firstLevel;

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
        CurrentState = GameState.Boot;
    }

    private void Start()
    {
        CurrentState = GameState.MainMenu;
        SceneController.Instance.LoadLevel(firstLevel);
    }

    /// <summary>
    /// Changes the current game state
    /// </summary>
    public void SetState(GameState newState)
    {
        CurrentState = newState;
    }

    /// <summary>
    /// Destroys the instance of the player and unloads all scenes.
    /// Then loads the first scene again
    /// </summary>
    public void ResetGame()
    {
        SceneController.Instance.UnloadNonPersistentScenes();
        SceneController.Instance.LoadLevel(firstLevel);
        CurrentState = GameState.MainMenu;
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

}
