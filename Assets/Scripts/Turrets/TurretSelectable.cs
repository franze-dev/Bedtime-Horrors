using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class TurretSelectable : MonoBehaviour, ITooltipInfo, IInteractable, IDraggable
{
    [SerializeField] private Turret _originalPrefab;
    private bool _isDragging;
    private GameObject _copy;

    public string Text => "CREATIVITY: " + _originalPrefab.price.ToString();

    private void Awake()
    {
        EventProvider.Subscribe<IClickEvent>(OnDragStart);
        EventProvider.Subscribe<IDragEvent>(OnDrag);
        EventProvider.Subscribe<IClickReleaseEvent>(OnDragEnd);
        _copy = null;
        _isDragging = false;
    }
    private void Update()
    {
        if (!_isDragging)
            return;

        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

        EventTriggerer.Trigger<IDragEvent>(new DragEvent(worldPos));
    }

    private void OnDestroy()
    {
        EventProvider.Unsubscribe<IClickEvent>(OnDragStart);
        EventProvider.Unsubscribe<IDragEvent>(OnDrag);
        EventProvider.Unsubscribe<IClickReleaseEvent>(OnDragEnd);
    }

    public void Interact()
    {
        EventTriggerer.Trigger<ISelectTurretPrefabEvent>(new SelectTurretPrefabEvent(this));
    }

    public void OnDragStart(IClickEvent @event)
    {
        if (!@event.HasHit) return;

        foreach (var hit in @event.AllHits)
        {
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                if (_copy == null)
                {
                    _copy = Instantiate(gameObject);
                    _copy.transform.position = transform.position;
                }

                _isDragging = true;
                break;
            }
        }


    }

    public void OnDrag(IDragEvent @event)
    {
        if (!_isDragging) return;
        _copy.transform.position = @event.MouseWorldPos;
    }

    public void OnDragEnd(IClickReleaseEvent @event)
    {
        if (!_isDragging)
            return;

        _isDragging = false;

        if (!@event.HasHit)
        {
            Debug.Log("No hit detected on drag end");
            Destroy(_copy);
            _copy = null;
            return;
        }

        foreach (var hit in @event.AllHits)
        {
            var spawner = hit.collider.GetComponent<TurretSpawner>();
            if (spawner != null)
            {
                spawner.Interact();
                break;
            }
        }

        Destroy(_copy);
        _copy = null;
    }
}


public class SelectTurretPrefabEvent : ISelectTurretPrefabEvent
{
    private TurretSelectable _turretSelectable;
    public GameObject TriggeredByGO => null;

    public TurretSelectable TurretSelectable => _turretSelectable;

    public SelectTurretPrefabEvent(TurretSelectable turretSelectable)
    {
        this._turretSelectable = turretSelectable;
    }
}

public interface ISelectTurretPrefabEvent : IEvent
{
    TurretSelectable TurretSelectable { get; }
}