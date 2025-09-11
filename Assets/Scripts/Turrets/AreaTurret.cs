using System.Collections.Generic;
using UnityEngine;

public class AreaTurret : Turret, IAreaTurret
{
    [SerializeField] private List<GameObject> _enemiesCollided;
    [SerializeField] private float _damage = 30;

    public List<GameObject> EnemiesCollided { get => _enemiesCollided; private set => _enemiesCollided = value; }

    protected override void Awake()
    {
        EnemiesCollided = new List<GameObject>();

        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
        ClearEnemyList();

        if (_timer >= cooldown && EnemiesCollided.Count > 0)
        {
            _timer = 0f;

            for (int i = 0; i < EnemiesCollided.Count; i++)
            {
                GameObject enemyGO = EnemiesCollided[i];
                var enemy = enemyGO?.GetComponent<Enemy>();
                
                enemy?.TakeDamage(_damage);
            }
        }
    }

    public void CollisionEnter(Collision2D collision)
    {
        EnemiesCollided?.Add(collision.gameObject);
    }

    public void CollisionExit(Collision2D collision)
    {
        if (!collision.gameObject.GetComponent<Enemy>())
            return;

        EnemiesCollided?.Remove(collision.gameObject);
    }


    private void ClearEnemyList()
    {
        for (int i = 0; i < EnemiesCollided.Count; i++)
        {
            if (!EnemiesCollided[i].gameObject.activeSelf)
            {
                EnemiesCollided.Remove(EnemiesCollided[i]);
                return;
            }
        }
    }
}
