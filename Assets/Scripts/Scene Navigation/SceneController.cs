using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public LevelContainer levelContainer;
    [SerializeField] private Level _bootLevel;

    private List<SceneRef> _loadedScenes = new();
    private List<SceneRef> _persistentLoadedScenes = new();

    private SceneRef _currentActiveScene;
    private SceneRef _previousActiveScene;
    public SceneRef CurrentActiveScene => _currentActiveScene;
    public SceneRef PreviousActiveScene => _previousActiveScene;


    private Level _currentActiveLevel;
    private Level _previousActiveLevel;
    public Level CurrentActiveLevel => _currentActiveLevel;
    public Level PreviousActiveLevel => _previousActiveLevel;

    public static SceneController Instance { get; private set; }

    /// <summary>
    /// Initializes the singleton instance of the SceneController.
    /// Destroys duplicates and persists across scenes.
    /// </summary>
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SaveBootScenes();
    }

    /// <summary>
    /// Loads the given level and unloads all non-persistent scenes currently loaded.
    /// </summary>
    public void LoadLevel(Level level)
    {
        AddLevel(level);

        foreach (var scene in _loadedScenes)
        {
            if (!scene.IsPersistent && !level.scenes.Contains(scene))
                UnloadSceneByIndex(scene.Index);
        }

    }

    /// <summary>
    /// Loads all scenes in the level additively if they aren't already loaded.
    /// Stores all scenes in private loadedScenes list
    /// Stores persistent scenes in private persistentLoadedScenes list
    /// </summary>
    public void AddLevel(Level level)
    {
        _previousActiveLevel = _currentActiveLevel;
        _currentActiveLevel = level;

        foreach (var scene in level.scenes)
        {
            if (!_loadedScenes.Contains(scene) && !_persistentLoadedScenes.Contains(scene))
            {
                LoadAdditiveByRef(scene);
                _loadedScenes.Add(scene);

                if (scene.IsPersistent)
                    _persistentLoadedScenes.Add(scene);
            }
        }
    }

    /// <summary>
    /// Unloads all scenes in the stack and clears the private lists
    /// </summary>
    public void UnloadAllScenes()
    {
        var scenesToUnload = new List<SceneRef>(_loadedScenes);

        foreach (var scene in scenesToUnload)
        {
            if (scene.Index != SceneManager.GetActiveScene().buildIndex)
                UnloadSceneByIndex(scene.Index);
        }

        _loadedScenes.Clear();
        _persistentLoadedScenes.Clear();
    }

    /// <summary>
    /// Finds a persistent scene and sets it active.
    /// Then unloads all the non persitent ones
    /// </summary>
    public void UnloadNonPersistentScenes()
    {
        _previousActiveLevel = _currentActiveLevel;

        SceneRef newActiveScene = null;

        foreach (var scene in _loadedScenes)
        {
            if (scene.IsActive)
            {
                newActiveScene = scene;
                SetSceneActive(scene);
                break;
            }
        }

        foreach (var scene in _loadedScenes)
        {
            if (!scene.IsPersistent)
                UnloadSceneByIndex(scene.Index);
        }

        _loadedScenes.RemoveAll(scene => !scene.IsPersistent);

        if (newActiveScene != null)
            _currentActiveLevel = GetLevelContainingScene(newActiveScene);
    }

    /// <summary>
    /// Save the scenes in the boot level
    /// </summary>
    public void SaveBootScenes()
    {
        foreach (var scene in _bootLevel.scenes)
        {
            if (!_loadedScenes.Contains(scene) && !_persistentLoadedScenes.Contains(scene))
                _loadedScenes.Add(scene);

            if (scene.IsPersistent && !_persistentLoadedScenes.Contains(scene))
                _persistentLoadedScenes.Add(scene);
        }
    }

    /// <summary>
    /// Starts coroutine to load a scene additively from a SceneRef
    /// </summary>
    public void LoadAdditiveByRef(SceneRef scene)
    {
        StartCoroutine(LoadAdditiveByRefRoutine(scene));
    }

    /// <summary>
    /// Coroutine that loads a scene additively and sets it active if needed.
    /// Also ensures duplicated AudioListeners are destroyed
    /// </summary>
    private IEnumerator LoadAdditiveByRefRoutine(SceneRef scene)
    {
        if (scene.Index < SceneManager.sceneCountInBuildSettings)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene.Index, LoadSceneMode.Additive);
            while (!asyncLoad.isDone)
                yield return null;

            Scene newScene = SceneManager.GetSceneByBuildIndex(scene.Index);
            if (newScene.IsValid() && scene.IsActive)
                SetSceneActive(scene);
        }
    }

    /// <summary>
    /// Starts coroutine to unload a scene by its build index
    /// </summary>
    public void UnloadSceneByIndex(int index)
    {
        StartCoroutine(UnloadSceneByIndexRoutine(index));
    }

    /// <summary>
    /// Coroutine that unloads a scene asynchronously by index
    /// </summary>
    private IEnumerator UnloadSceneByIndexRoutine(int index)
    {
        if (index < 0 || index >= SceneManager.sceneCountInBuildSettings)
            yield break;

        Scene scene = SceneManager.GetSceneByBuildIndex(index);
        if (!scene.isLoaded)
            yield break;

        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(index);
        if (asyncUnload == null)
            yield break;

        while (!asyncUnload.isDone)
            yield return null;
    }

    /// <summary>
    /// Sets the scene with the given build index as the active scene,
    /// and updates the current and previous active SceneRefs
    /// </summary>
    public void SetSceneActive(SceneRef sceneToLoad)
    {
        Scene scene = SceneManager.GetSceneByBuildIndex(sceneToLoad.Index);
        if (scene.IsValid())
        {
            SceneManager.SetActiveScene(scene);

            SceneRef sceneRef = _loadedScenes.Find(s => s.Index == sceneToLoad.Index);
            if (sceneRef != null)
            {
                _previousActiveScene = _currentActiveScene;
                _currentActiveScene = sceneRef;
            }
        }
    }

    public bool IsGameplaySceneActive()
    {
        if (_currentActiveScene == null)
            return false;
        return IsSceneInGameplayLevels(_currentActiveScene);
    }

    private bool IsSceneInGameplayLevels(SceneRef sceneRef)
    {
        foreach (var level in levelContainer.gameplayLevels)
        {
            if (level.scenes.Contains(sceneRef))
                return true;
        }
        return false;
    }

    /// <summary>
    /// Exits the application. Stops play mode if running in the Unity Editor
    /// </summary>
    public void Exit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private Level GetLevelContainingScene(SceneRef sceneRef)
    {
        if (levelContainer.bootLevel != null &&
            levelContainer.bootLevel.scenes.Contains(sceneRef))
            return levelContainer.bootLevel;

        foreach (var level in levelContainer.gameplayLevels)
        {
            if (level.scenes.Contains(sceneRef))
                return level;
        }

        if (levelContainer.menusLevel != null &&
            levelContainer.menusLevel.scenes.Contains(sceneRef))
            return levelContainer.menusLevel;

        return null;
    }

}
