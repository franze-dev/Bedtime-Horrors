using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class TurretSelectable : MonoBehaviour, IInteractable, IDraggable
{
    private bool _isDragging;
    private GameObject _copy;



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
        Debug.Log("INTREACTUOOOOOO");
    }

    public void OnDragStart(IClickEvent @event)
    {
        if (_copy == null)
        {
            //Debug.Log("INSTANCIANDO");
            _copy = Instantiate(gameObject);
            _copy.transform.position = transform.position;
        }

        if (!@event.HasHit) return;

        foreach (var hit in @event.AllHits)
        {
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                _isDragging = true;
                break;
            }
        }


    }

    public void OnDrag(IDragEvent @event)
    {
        if (!_isDragging) return;
        _copy.transform.position = @event.MouseWorldPos;

        Debug.Log("Dragging");
    }

    public void OnDragEnd(IClickReleaseEvent @event)
    {
        if (!_isDragging)
            return;

        //Debug.Log("Drag released");

        _isDragging = false;

        if (!@event.HasHit)
        {
            Debug.Log("No hit detected on drag end");
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