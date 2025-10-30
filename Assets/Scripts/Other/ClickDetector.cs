using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(ClickHitDetector))]
public class ClickDetector : MonoBehaviour
{
    [SerializeField] private InputActionReference _click;

    private void OnEnable()
    {
        _click.action.started += OnClick;
        _click.action.canceled += OnClickRelease;
    }

    private void OnDisable()
    {
        _click.action.started -= OnClick;
        _click.action.canceled -= OnClickRelease;
    }

    private void OnClick(InputAction.CallbackContext context)
    {
        EventTriggerer.Trigger<IClickEvent>(new ClickHitEvent(Mouse.current.position.ReadValue()));
    }

    private void OnClickRelease(InputAction.CallbackContext context)
    {
        EventTriggerer.Trigger<IClickReleaseEvent>(new ClickReleaseEvent(Mouse.current.position.ReadValue()));
    }
}

public class ClickHitEvent : IClickEvent
{
    private RaycastHit2D _hit;
    private RaycastHit2D[] _allHits;
    private Vector2 _mouseInputPos;
    private Vector2 _mouseScreenPos;

    public GameObject TriggeredByGO => null;

    public RaycastHit2D FirstHit => _hit;
    public RaycastHit2D[] AllHits => _allHits;
    public bool HasHit => _hit.collider != null;
    public Vector2 MouseInputPos => _mouseInputPos;
    public Vector2 MouseScreenPos => _mouseScreenPos;

    public ClickHitEvent(Vector2 mousePosInput)
    {
        _mouseInputPos = mousePosInput;

        if (Camera.main != null)
            _mouseScreenPos = Camera.main.ScreenToWorldPoint(mousePosInput);
        else
            _mouseScreenPos = mousePosInput;

        _allHits = Physics2D.RaycastAll(_mouseScreenPos, Vector2.zero);

        if (_allHits.Length > 0)
            _hit = AllHits[0];
    }
}

public class ClickReleaseEvent : IClickReleaseEvent
{
    private RaycastHit2D _hit;
    private RaycastHit2D[] _allHits;
    private Vector2 _mouseInputPos;
    private Vector2 _mouseScreenPos;

    public GameObject TriggeredByGO => null;
    public Vector2 MouseWorldPos { get; }

    public RaycastHit2D FirstHit => _hit;
    public RaycastHit2D[] AllHits => _allHits;
    public bool HasHit => _hit.collider != null;
    public Vector2 MouseInputPos => _mouseInputPos;
    public Vector2 MouseScreenPos => _mouseScreenPos;

    public ClickReleaseEvent(Vector2 mousePosInput)
    {
        _mouseInputPos = mousePosInput;

        if (Camera.main != null)
            _mouseScreenPos = Camera.main.ScreenToWorldPoint(mousePosInput);
        else
            _mouseScreenPos = mousePosInput;

        _allHits = Physics2D.RaycastAll(_mouseScreenPos, Vector2.zero);

        if (_allHits.Length > 0)
            _hit = AllHits[0];
    }
}

public interface IClickEvent : IEvent
{
    RaycastHit2D FirstHit { get; }
    public RaycastHit2D[] AllHits { get; }
    bool HasHit { get; }
    Vector2 MouseInputPos { get; }
    Vector2 MouseScreenPos { get; }
}

public interface IClickReleaseEvent : IClickEvent
{

}

