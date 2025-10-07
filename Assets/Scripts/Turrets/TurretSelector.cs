using UnityEngine;
using UnityEngine.InputSystem;

public class TurretSelector : MonoBehaviour
{
    [SerializeField] InputActionReference _upDir;
    [SerializeField] InputActionReference _downDir;
    [SerializeField] InputActionReference _leftDir;
    [SerializeField] InputActionReference _rightDir;

    [SerializeField] private BasicTurret _selectedTurret;

    void Start()
    {
        if (_selectedTurret == null)
            return;

        _upDir.action.started += OnChangeDirUp;
        _downDir.action.started += OnChangeDirDown;
        _leftDir.action.started += OnChangeDirLeft;
        _rightDir.action.started += OnChangeDirRight;
    }

    private void OnChangeDirUp(InputAction.CallbackContext context)
    {
        if (!TryGetSelectedTurret())
            return;

        _selectedTurret.ChangeDir(Vector2.up);
    }

    private void OnChangeDirDown(InputAction.CallbackContext context)
    {
        if (!TryGetSelectedTurret())
            return;

        _selectedTurret.ChangeDir(Vector2.down);
    }

    private void OnChangeDirLeft(InputAction.CallbackContext context)
    {
        if (!TryGetSelectedTurret())
            return;

        _selectedTurret.ChangeDir(Vector2.left);
    }

    private void OnChangeDirRight(InputAction.CallbackContext context)
    {
        if (!TryGetSelectedTurret())
            return;

        _selectedTurret.ChangeDir(Vector2.right);
    }

    private bool TryGetSelectedTurret()
    {
        if (this == null)
            return false;

        if (!gameObject.activeSelf)
            return false;

        if (_selectedTurret == null)
            _selectedTurret = transform.parent.GetComponent<BasicTurret>();

        return _selectedTurret != null;
    }
}
