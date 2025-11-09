using DragonBones;
using UnityEngine;
using UnityEngine.InputSystem;

public class TurretSelectable : MonoBehaviour, ITooltipInfo, IInteractable, IDraggable
{
    [SerializeField] private Turret _originalPrefab;

    [SerializeField] private UnityArmatureComponent _armature;
    [SerializeField] private CreativityUpdater _creativityUpdater;
    [SerializeField] private Color _originalColor;
    [SerializeField] private Color _unaffordableColor;

    private bool _isDragging;
    private GameObject _copy;

    public string Text => "CREATIVITY: " + _originalPrefab.price.ToString();

    private void Awake()
    {
        EventProvider.Subscribe<IClickEvent>(OnDragStart);
        EventProvider.Subscribe<IDragEvent>(OnDrag);
        EventProvider.Subscribe<IClickReleaseEvent>(OnDragEnd);

        if (_armature == null)
            _armature = GetComponentInChildren<UnityArmatureComponent>();

        _copy = null;
        _isDragging = false;
    }
    private void Update()
    {
        if (_armature == null)
            Debug.LogWarning("Armature is null");

        if (_creativityUpdater == null)
            Debug.LogWarning("Creativity updater is null");

        if (_originalPrefab.HasEnoughCreativity(_creativityUpdater, _originalPrefab.price))
            SetArmatureColor(_originalColor);
        else
            SetArmatureColor(_unaffordableColor);

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

    private void SetArmatureColor(Color color)
    {
        if (_armature == null || _armature.armature == null)
            return;

        var ct = new ColorTransform
        {
            alphaMultiplier = color.a,
            redMultiplier = color.r,
            greenMultiplier = color.g,
            blueMultiplier = color.b
        };

        _armature.color = ct;
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