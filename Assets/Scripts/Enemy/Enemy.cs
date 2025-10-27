using System.Collections;
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
    [SerializeField] private GameObject _armatureGO;

    [SerializeField] private EnemyAnimator _animator;

    private FloatingText _floatingText;

    public bool IsDead => _isDead;

    private void Awake()
    {
        if (_targetManager == null)
            Debug.LogError("No TargetManager assigned to " + gameObject.name);

        if(_animator == null)
            _animator = GetComponent<EnemyAnimator>();
    }

    void OnEnable()
    {
        _currentTargetIndex = 0;
        _currentTarget = _targetManager.Targets[_currentTargetIndex];

        if (_healthBar == null)
            _healthBar = GetComponentInChildren<SliderUpdater>();

        if (_collider == null)
            _collider = GetComponent<BoxCollider2D>();

        _isDead = false;

        if (_spriteRenderer == null)
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (!_spriteRenderer && !_armatureGO)
            Debug.LogError("No SpriteRenderer or Armature assigned to " + gameObject.name);

        if (_armatureGO != null)
            _armatureGO.SetActive(true);
        else if (_spriteRenderer != null)
            _spriteRenderer.gameObject.SetActive(true);

        _healthBar.gameObject.SetActive(true);
        _collider.enabled = true;

        _floatingText = _floatingDamage.GetComponent<FloatingText>();

        _currentHealth = _maxHealth;
        _healthBar.UpdateSlider(_currentHealth, _maxHealth);

        if (_animator != null)
            _animator.Play(EnemyAnimationState.Walk);

        EventTriggerer.Trigger<IEnemySpawnEvent>(new EnemySpawnEvent());
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
            if (collision.gameObject.TryGetComponent(out Bullet bullet))
            {
                float damageToTake = bullet.damage;
                TakeDamage(damageToTake);
                return;
            }
        }
    }

    public void OnDeath()
    {
        EventTriggerer.Trigger<ICreativityUpdateEvent>(new CreativityUpdaterEvent(gameObject, _creativityToSum));
        _isDead = true;
        _animator.Play(EnemyAnimationState.Death, 1);
        MultiplySpeed(0f);
        StartCoroutine(DeathCoroutine());
    }

    private IEnumerator DeathCoroutine()
    {
        if (_spriteRenderer != null)
            _spriteRenderer.gameObject.SetActive(false);

        _healthBar.gameObject.SetActive(false);
        _collider.enabled = false;

        yield return new WaitForSeconds(_animator.GetAnimationDuration(EnemyAnimationState.Death));

        EventTriggerer.Trigger<IEnemyDeathEvent>(new EnemyDeathEvent());
        ResetSpeed();
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        ResetSpeed();
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

public class EnemySpawnEvent : IEnemySpawnEvent
{
    public GameObject TriggeredByGO => null;

    public bool IsSequencePanel => true;

    public EnemySpawnEvent()
    {
        EventTriggerer.Trigger<IContinuePanelsEvent>(new ContinuePanelsEvent());
    }
}

internal interface IEnemySpawnEvent : IEvent
{
}

public class EnemyDeathEvent : IEnemyDeathEvent
{
    public GameObject TriggeredByGO => null;

    public EnemyDeathEvent()
    {
        EventTriggerer.Trigger<IContinuePanelsEvent>(new ContinuePanelsEvent());
    }
}

public interface IEnemyDeathEvent : IEvent
{
}