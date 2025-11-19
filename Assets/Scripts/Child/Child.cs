using UnityEngine;

public class Child : MonoBehaviour
{
    [SerializeField] private Transform startPos;

    [SerializeField] private float _maxHealth;
    [SerializeField] private float _currentHealth;
    [SerializeField] private SliderUpdater _healthBar;

    [SerializeField] private GameObject _floatingDamage;


    private void Awake()
    {
        _currentHealth = _maxHealth;
        transform.position = startPos.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
        {
            float damageToTake = enemy.GetDamage();
            TakeDamage(damageToTake);
            Debug.Log(damageToTake + " damage taken!");
            AkUnitySoundEngine.PostEvent("Base_TakeDamage", gameObject);

            enemy?.OnDeath();
            return;
        }
    }

    private void TakeDamage(float damage)
    {
        var msg = Instantiate(_floatingDamage, transform.position, Quaternion.identity, gameObject.transform);
        msg.transform.localPosition = Vector2.zero;
        msg.transform.localScale = Vector2.one * 2; //Hacer esto bien en el futuro

        msg.GetComponent<FloatingText>()?.SetText(damage.ToString());

        _currentHealth -= damage;

        if (_currentHealth < Mathf.Epsilon)
        {
            Debug.Log("You lost!!");
            _currentHealth = 0;

            SceneController.Instance.UnloadNonPersistentScenes();

            if (ServiceProvider.TryGetService(out NavigationController nav))
            {
                AkUnitySoundEngine.StopAll();
                nav.GoToMenu(new LoseMenuState());
            }
            else
                Debug.LogWarning("NavigationController service not found!");
        }

        _healthBar.UpdateSlider(_currentHealth, _maxHealth);
    }

    public float GetMaxHealth()
    {
        return _maxHealth;
    }

    public float GetCurrentHealth()
    {
        return _currentHealth;
    }
}
