using UnityEngine;

public class Turret : MonoBehaviour, IInteractable
{
    [SerializeField] protected float cooldown;
    [SerializeField] private GameObject _selectionGO;
    [SerializeField] private AreaNotifier _areaNotifier;
    [SerializeField] protected float damage = 30;
    protected float _timer = 0;
    public int price;
    public SpriteRenderer spriteRenderer;
    [SerializeField] private SpriteRenderer _areaSpriteRenderer;
    private float _areaTransparency;
    private BoxCollider2D _collider;


    protected virtual void Awake()
    {
        EventTriggerer.Trigger<ITurretSpawnEvent>(new TurretSpawnEvent(this.gameObject));

        if (_selectionGO == null)
            Debug.LogError("Selection GO not found on " + gameObject.name);
        else
            _selectionGO.SetActive(false);

        if (!_areaNotifier)
            _areaNotifier = GetComponentInChildren<AreaNotifier>();

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if (_areaSpriteRenderer != null)
            _areaTransparency = _areaSpriteRenderer.color.a;

        _collider = GetComponentInChildren<BoxCollider2D>();

        EventProvider.Subscribe<IClickEvent>(OnClickAny);
    }

    private void OnClickAny(IClickEvent @event)
    {
        if (!@event.HasHit)
            EventTriggerer.Trigger<IUpdateSelectedTurretEvent>(new UpdateSelectedTurret(null));
    }

    public void Interact()
    {
        EventTriggerer.Trigger<IUpdateSelectedTurretEvent>(new UpdateSelectedTurret(this));
    }

    protected virtual void Update()
    {
        _timer += Time.deltaTime;
    }

    public void Select()
    {
        _selectionGO.SetActive(true);
    }

    public void Deselect()
    {
        _selectionGO.SetActive(false);
    }

    public void ActivateArea()
    {
        if (_areaSpriteRenderer == null)
            return;

        Color areaColor = _areaSpriteRenderer.color;
        areaColor.a = _areaTransparency;
        _areaSpriteRenderer.color = areaColor;
    }

    public void DeactivateArea()
    {
        if (_areaSpriteRenderer == null)
            return;

        Color areaColor = _areaSpriteRenderer.color;
        areaColor.a = 0;
        _areaSpriteRenderer.color = areaColor;
    }

}

public interface TurretStats
{

}