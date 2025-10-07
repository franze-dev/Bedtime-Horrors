using UnityEngine;

[CreateAssetMenu(fileName = "SceneRef", menuName = "ScriptableObjects/SceneRef")]
public class SceneRef : ScriptableObject
{
    [SerializeField] private int _index;
    [SerializeField] private bool _isActive;
    [SerializeField] private bool _isPersistent;

    public int Index { get => _index; }
    public bool IsActive { get => _isActive; }
    public bool IsPersistent { get => _isPersistent; }
}
