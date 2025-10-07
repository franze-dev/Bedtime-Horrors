using UnityEngine;


#if UNITY_EDITOR
public class EditorCamera : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    private void Awake()
    {
        _camera.gameObject.SetActive(true);
        Camera.SetupCurrent(_camera);
    }
}

#endif