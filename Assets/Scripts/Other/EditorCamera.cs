using UnityEngine;

public class EditorCamera : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    private void Awake()
    {
#if UNITY_EDITOR
        _camera.gameObject.SetActive(true);
        Camera.SetupCurrent(_camera);
#endif
    }
}
