#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "SceneRef", menuName = "ScriptableObjects/SceneRef")]
public class SceneRef : ScriptableObject
{
    [SerializeField] private int _index;
    [SerializeField] private bool _isActive;
    [SerializeField] private bool _isPersistent;

    public int Index { get => _index; }
    public bool IsActive { get => _isActive; }
    public bool IsPersistent { get => _isPersistent; }

#if UNITY_EDITOR
    [SerializeField] private SceneAsset _sceneAsset;

    private void OnValidate()
    {
        _index = SceneUtility.GetBuildIndexByScenePath(AssetDatabase.GetAssetPath(_sceneAsset));
    }
#endif
}
