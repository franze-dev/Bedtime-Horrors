using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] protected float cooldown;
    protected float _timer = 0;
    public int price;

    protected virtual void Awake()
    {
        EventTriggerer.Trigger<ITurretSpawnEvent>(new TurretSpawnEvent(this.gameObject));
    }

    protected virtual void Update()
    {
        _timer += Time.deltaTime;
    }
}
