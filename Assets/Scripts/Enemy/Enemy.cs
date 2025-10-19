using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private TargetManager _targetManager;
    [SerializeField] private float _speed;
    [SerializeField] private float _damage;

    [SerializeField] private float _maxHealth;
    [SerializeField] private float _currentHealth;
    [SerializeField] private int _creativityToSum = 10;

    private bool _isDead;

    private GameObject _currentTarget;
    private int _currentTargetIndex;
    private float _speedMultiplier = 1f;

    [SerializeField] private SliderUpdater _healthBar;
    [SerializeField] private Transform _floatingDamageSpawn;
    [SerializeField] private GameObject _floatingDamage;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private BoxCollider2D _collider;
    [SerializeField] private GameObject _armature;
    [SerializeField] private PanelEventTrigger _spawnPanel;
    [SerializeField] private PanelEventTrigger _deathPanel;

    private FloatingText _floatingText;

    public bool IsDead => _isDead;

    private void Awake()
    {
        if (_targetManager == null)
            Debug.LogError("No TargetManager assigned to " + gameObject.name);

        if (_spawnPanel == null)
            Debug.LogError("No Spawn Panel assigned to " + gameObject.name);

        if (_deathPanel == null)
            Debug.LogError("No Death Panel assigned to " + gameObject.name);
    }

    void OnEnable()
    {
        _spawnPanel.TriggerEvent();

        _currentTargetIndex = 0;
        _currentTarget = _targetManager.Targets[_currentTargetIndex];

        if (_healthBar == null)
            _healthBar = GetComponentInChildren<SliderUpdater>();

        if (_collider == null)
            _collider = GetComponent<BoxCollider2D>();

        _isDead = false;

        if (_spriteRenderer == null)
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (!_spriteRenderer && !_armature)
            Debug.LogError("No SpriteRenderer or Armature assigned to " + gameObject.name);

        if (_armature != null)
            _armature.SetActive(true);
        else if (_spriteRenderer != null)
            _spriteRenderer.gameObject.SetActive(true);

        _healthBar.gameObject.SetActive(true);
        _collider.enabled = true;

        _floatingText = _floatingDamage.GetComponent<FloatingText>();

        _currentHealth = _maxHealth;
        _healthBar.UpdateSlider(_currentHealth, _maxHealth);

        Debug.Log(gameObject.name + " Enabled!");
        Debug.Log("Current target set to " + _currentTargetIndex);

    }

    void Update()
    {
        if (_currentTarget != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, _currentTarget.transform.position, _speed * _speedMultiplier * Time.deltaTime);
            CheckTargetReached();
        }
    }

    private void CheckTargetReached()
    {
        float distance = Vector2.Distance(transform.position, _currentTarget.transform.position);

        if (distance < Mathf.Epsilon + 0.0001f)
        {
            if (_currentTargetIndex < _targetManager.Targets.Count - 1)
            {
                _currentTargetIndex++;
                _currentTarget = _targetManager.Targets[_currentTargetIndex];
            }
            else if (_currentTargetIndex == _targetManager.Targets.Count - 1)
                _currentTarget = _targetManager.FinalTarget;
            else
                _currentTarget = null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("bullet"))
        {
            if (collision.gameObject.TryGetComponent<Bullet>(out Bullet bullet))
            {
                float damageToTake = bullet.damage;
                TakeDamage(damageToTake);
                Debug.Log(damageToTake + " damage taken!");
                return;
            }
        }
        Debug.Log("This enemy collided with another object");
    }

    public void OnDeath()
    {
        _deathPanel.TriggerEvent();
        _isDead = true;
        StartCoroutine(DeathCoroutine());
    }

    private IEnumerator DeathCoroutine()
    {
        if (_armature != null)
            _armature.SetActive(false);
        else if (_spriteRenderer != null)
            _spriteRenderer.gameObject.SetActive(false);

        _healthBar.gameObject.SetActive(false);
        _collider.enabled = false;

        yield return new WaitForSeconds(_floatingText.LifeTime);

        this.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        ResetSpeed();
        EventTriggerer.Trigger<ICreativityUpdateEvent>(new CreativityUpdaterEvent(this.gameObject, _creativityToSum));
    }

    public void TakeDamage(float damage)
    {
        var msg = Instantiate(_floatingDamage, _floatingDamageSpawn.position, Quaternion.identity, gameObject.transform);
        msg.transform.localPosition = Vector2.zero;
        msg.transform.localScale = Vector2.one * 3; //Hacer esto bien en el futuro

        Color damageColor = GetDamageColor(damage);

        msg.GetComponent<FloatingText>()?.SetTextAndColor(damage.ToString(), damageColor);

        _currentHealth -= damage;

        if (_currentHealth < Mathf.Epsilon)
            OnDeath();

        _healthBar.UpdateSlider(_currentHealth, _maxHealth);
    }

    private Color GetDamageColor(float damage)
    {
        float minDamage = StaticTurretValues.minDamage;
        float maxDamage = StaticTurretValues.maxDamage;
        float t = Mathf.InverseLerp(minDamage, maxDamage, damage);
        Color damageColor = Color.Lerp(Color.green, Color.red, t);
        return damageColor;
    }

    public float GetDamage()
    {
        return _damage;
    }

    public void MultiplySpeed(float speedMultiplier)
    {
        _speedMultiplier = speedMultiplier;
    }

    public void ResetSpeed()
    {
        _speedMultiplier = 1;
    }

    public float GetMaxHealth()
    {
        return _maxHealth;
    }

    public void SetCurrentHealth(float value)
    {
        _currentHealth = value;
    }
}