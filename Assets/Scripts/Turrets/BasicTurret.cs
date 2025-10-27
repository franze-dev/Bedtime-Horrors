using UnityEngine;

public class BasicTurret : ShootTurret
{
    [SerializeField] private Vector2 _direction;
    [SerializeField] private GameObject _arrowGO;

    public override void Fire(float damage)
    {
        if (Bullets.Count < MaxBullets)
            AddNewBullet(_direction, damage);
        else
            RetargetBullets();
    }

    protected override void Awake()
    {
        if (BulletGO == null)
            Debug.LogError("BulletGO not found");

        if (_arrowGO == null)
            Debug.LogError("Directional arrow missing in " + gameObject.name);

        _arrowGO.SetActive(true);

        ChangeDir(_direction);

        base.Awake();
    }

    protected override void Update()
    {
        base.Update();

        if (_timer >= cooldown)
        {
            Fire(_direction, damage);
            _timer = 0f;
        }
    }

    public void ChangeDir(Vector2 dir)
    {
        _arrowGO.transform.position = (Vector2)transform.position + dir * 0.5f;

        _arrowGO.transform.rotation = Quaternion.identity;

        var angle = Vector3.Angle(_arrowGO.transform.up, dir);

        angle *= dir.x != 0 ? -dir.x : dir.y;

        _arrowGO.transform.Rotate(new(0, 0, angle));

        _direction = dir;

        foreach (var bullet in Bullets)
            bullet.GetComponent<Bullet>().nextDirection = _direction;
    }
}
