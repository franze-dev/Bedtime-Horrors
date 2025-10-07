using TMPro;
using UnityEngine;

public class ChildHealthBar : MonoBehaviour
{
    [SerializeField] private SliderUpdater _healthBar;
    [SerializeField] private Child _child;

    [SerializeField] private float _childMaxHealth;
    [SerializeField] private float _childCurrentHealth;


    [SerializeField] private TextUpdater _textUpdater;
    private string _healthText;

    private void Awake()
    {
        _childMaxHealth = _child.GetMaxHealth();
        _childCurrentHealth = _child.GetCurrentHealth();

         _healthText = _childCurrentHealth.ToString() + " / " + _childMaxHealth.ToString();
        _textUpdater.ChangeText(_healthText);
    }

    private void Update()
    {
        _childCurrentHealth = _child.GetCurrentHealth();

        _healthText = _childCurrentHealth.ToString() + " / " + _childMaxHealth.ToString();
        _textUpdater.ChangeText(_healthText);
    }

}
