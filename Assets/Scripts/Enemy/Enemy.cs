using Unity.Mathematics;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    [SerializeField] private TargetManager _TargetManager;
    [SerializeField] private float _speed;
    [SerializeField] private float _damage;

    [SerializeField] private float _maxHealth;
    [SerializeField] private float _currentHealth;
    [SerializeField] private int _creativityToSum = 10;

    private GameObject _currentTarget;
    private int _currentTargetIndex;
    private float _speedMultiplier = 1f;

    [SerializeField] private SliderUpdater _healthBar;
    [SerializeField] private Transform _floatingDamageSpawn;
    [SerializeField] private GameObject _floatingDamage;

    void OnEnable()
    {
        _currentTargetIndex = 0;
        _currentTarget = _TargetManager.Targets[_currentTargetIndex];

        if (_healthBar == null)
            _healthBar = GetComponentInChildren<SliderUpdater>();

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

        if (distance < math.EPSILON)
        {
            if (_currentTargetIndex < _TargetManager.Targets.Count - 1)
            {
                _currentTargetIndex++;
                _currentTarget = _TargetManager.Targets[_currentTargetIndex];
            }
            else if (_currentTargetIndex == _TargetManager.Targets.Count - 1)
                _currentTarget = _TargetManager.FinalTarget;
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

        msg.GetComponent<FloatingText>()?.SetText(damage.ToString());

        _currentHealth -= damage;

        if (_currentHealth < Mathf.Epsilon)
            this.gameObject.SetActive(false);

        _healthBar.UpdateSlider(_currentHealth, _maxHealth);
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