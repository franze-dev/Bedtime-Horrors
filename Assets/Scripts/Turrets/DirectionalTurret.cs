using System.Collections.Generic;
using UnityEngine;

public class DirectionalTurret : ShootTurret, IAreaTurret
{
    [SerializeField] private List<GameObject> _enemiesCollided;

    [SerializeField] private GameObject _currentTarget;

    public List<GameObject> EnemiesCollided { get => _enemiesCollided; private set => _enemiesCollided = value; }

    protected override void Awake()
    {
        if (BulletGO == null)
            Debug.LogError("BulletGO not found");

        EnemiesCollided = new List<GameObject>();

        base.Awake();
    }

    protected override void Update()
    {
        base.Update();

        ClearEnemyList();

        if (_timer >= cooldown && EnemiesCollided.Count > 0)
        {
            Fire();
            AkUnitySoundEngine.PostEvent("Tower_Shoot_TeddyBear", gameObject);
            _timer = 0f;
        }
    }

    public void CollisionEnter(Collision2D collision)
    {
        EnemiesCollided?.Add(collision.gameObject);

        EnemiesCollided[0] = EnemiesCollided[0] != null ? EnemiesCollided[0] : collision.gameObject;
        //_currentTarget

    }

    public void CollisionExit(Collision2D collision)
    {
        if (!collision.gameObject.GetComponent<Enemy>())
            return;

        EnemiesCollided?.Remove(collision.gameObject);
        //Debug.Log()
    }

    public override void Fire()
    {
        if (Bullets.Count < MaxBullets)
        {
            var target = EnemiesCollided[0] != null ? EnemiesCollided[0] : null;

            AddNewBullet(new(0, 0), target);
        }
        else
            RetargetBullets(EnemiesCollided[0]);
    }


    private void ClearEnemyList()
    {
        for (int i = 0; i < EnemiesCollided.Count; i++)
        {
            var enemy = EnemiesCollided[i].GetComponent<Enemy>();
            if (!EnemiesCollided[i].activeSelf || enemy.IsDead)
            {
                EnemiesCollided.Remove(EnemiesCollided[i]);
                return;
            }
        }
    }
}
