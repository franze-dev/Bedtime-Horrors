using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(ClickHitDetector))]
public class ClickDetector : MonoBehaviour
{
    [SerializeField] private InputActionReference _click;

    private void OnEnable()
    {
        _click.action.started += OnClick;
    }

    private void OnDisable()
    {
        _click.action.started -= OnClick;
    }

    private void OnClick(InputAction.CallbackContext context)
    {
        EventTriggerer.Trigger<IClickHitEvent>(new ClickHitEvent(Mouse.current.position.ReadValue()));
    }
}

public interface IClickHitEvent : IEvent
{
    RaycastHit2D FirstHit { get; }
    public RaycastHit2D[] AllHits { get; }
    bool HasHit { get; }
    Vector2 MouseInputPos { get; }
    Vector2 MouseScreenPos { get; }
}

public class ClickHitEvent : IClickHitEvent
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
