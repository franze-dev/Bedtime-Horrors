using UnityEngine;
using UnityEngine.InputSystem;

public class CursorFollower : MonoBehaviour
{
    private Vector2 _mousePos = Vector2.zero;
    [HideInInspector] public Vector2 MouseCurrentPos => _mousePos;
    
    private void Update()
    {
        _mousePos = Mouse.current.position.ReadValue();
        Vector3 mouseToScreenPos = Camera.main.ScreenToWorldPoint(_mousePos);
        mouseToScreenPos.z = 0;
        transform.position = mouseToScreenPos;
    }
}
