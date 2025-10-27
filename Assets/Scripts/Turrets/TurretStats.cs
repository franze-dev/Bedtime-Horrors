using UnityEngine;

[CreateAssetMenu(fileName = "TurretStats", menuName = "ScriptableObjects/TurretStats")]
public class TurretStats : ScriptableObject
{
    [SerializeField] private float _cooldown;
    [SerializeField] private float _range;
    [SerializeField] private float _damage;

    public float Cooldown { get => _cooldown; set => _cooldown = value; }
    public float Range { get => _range; set => _range = value; }
    public float Damage { get => _damage; set => _damage = value; }
}
