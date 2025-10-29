using System;
using UnityEngine;

public class Turret : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject _selectionGO;
    [SerializeField] private AreaNotifier _areaNotifier;
    [SerializeField] private string _name;
    [SerializeField] private TurretLevels _levelData;

    protected float timer = 0;
    public int price;
    private int _currentLevelId = 0;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private SpriteRenderer _areaSpriteRenderer;
    private float _areaTransparency;

    public TurretStats CurrentStats => _levelData.GetStats(_currentLevelId);
    public TurretStats NextStats => _levelData.GetStats(_currentLevelId + 1);
    public float LevelUpPrice => CurrentStats != null ? CurrentStats.LevelUpPrice : 0;
    public float Cooldown => CurrentStats != null ? CurrentStats.Cooldown : 0;
    public float Damage => CurrentStats != null ? CurrentStats.Damage : 0;
    public float Range => CurrentStats != null ? CurrentStats.Range : 0;
    public string Name { get => _name; set => _name = value; }
    public SpriteRenderer SpriteRenderer { get => spriteRenderer; set => spriteRenderer = value; }
    private SelectedTurretVisual _selectedTurretVisual;
    private CreativityUpdater _creativityUpdater;

    protected virtual void Awake()
    {
        EventTriggerer.Trigger<ITurretSpawnEvent>(new TurretSpawnEvent(this.gameObject));

        if (_selectionGO == null)
            Debug.LogError("Selection GO not found on " + gameObject.name);
        else
            _selectionGO.SetActive(false);

        if (!_areaNotifier)
            _areaNotifier = GetComponentInChildren<AreaNotifier>();

        SpriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (SpriteRenderer == null)
            SpriteRenderer = GetComponent<SpriteRenderer>();

        if (_areaSpriteRenderer != null)
            _areaTransparency = _areaSpriteRenderer.color.a;

        _currentLevelId = 0;

        EventProvider.Subscribe<IClickEvent>(OnClickAny);
    }

    private void Start()
    {
        if (_areaNotifier != null)
            _areaNotifier.SetRange(Range);

        ServiceProvider.TryGetService(out _selectedTurretVisual);
        ServiceProvider.TryGetService(out _creativityUpdater);
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
        timer += Time.deltaTime;
    }

    public void Select()
    {
        _selectionGO.SetActive(true);
        _selectedTurretVisual.SetStats(CurrentStats, NextStats, _name);
        _selectedTurretVisual.Enable();
    }

    public void Deselect()
    {
        _selectionGO.SetActive(false);
        _selectedTurretVisual.Disable();
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

    public bool Upgrade()
    {

        if (_currentLevelId < 0 || _currentLevelId >= _levelData.LevelCount - 1)
            return false;
        
        if (NextStats == null)
            return false;

        _currentLevelId++;

        if (!HasEnoughCreativity())
        {
            _currentLevelId--;
            return false;
        }

        _selectedTurretVisual.SetStats(CurrentStats, NextStats, _name);
        return true;
    }

    private bool HasEnoughCreativity()
    {
        return _creativityUpdater.GetCreativityValue() >= LevelUpPrice &&
               _creativityUpdater.GetCreativityValue() - LevelUpPrice >= 0;
    }
}