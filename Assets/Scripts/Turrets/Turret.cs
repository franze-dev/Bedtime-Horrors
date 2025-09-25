using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] protected float cooldown;
    [SerializeField] private GameObject _selectionGO;
    [SerializeField] private AreaNotifier _areaNotifier;
    protected float _timer = 0;
    public int price;
    public SpriteRenderer spriteRenderer;

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
        if (_areaNotifier != null)
            _areaNotifier.gameObject.SetActive(true);
    }

    public void DeactivateArea()
    {
        if (_areaNotifier != null)
            _areaNotifier.gameObject.SetActive(false);
    }
}
