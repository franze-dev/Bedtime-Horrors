using UnityEngine;

[CreateAssetMenu(fileName = "PathSettings", menuName = "ScriptableObjects/Path/PathSettings")]
public class PathSettings : ScriptableObject
{
    [SerializeField] private GameObject pathPrefab;
    public GameObject PathPrefab => pathPrefab;
}
