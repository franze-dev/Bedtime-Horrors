using UnityEngine;

public class BasicTurret : ShootTurret
{
    [SerializeField] private Vector2 _direction;
    [SerializeField] private GameObject _arrowGO;
    [SerializeField] private Sprite _sprite;

    public override void Fire()
    {
        if (Bullets.Count < MaxBullets)
            AddNewBullet(_direction);
        else
            RetargetBullets();
    }

    protected override void Awake()
    {
        if (BulletGO == null)
            Debug.LogError("BulletGO not found");

        if (_arrowGO == null)
            Debug.LogError("Directional arrow missing in " + gameObject.name);

        _sprite = GetComponentInChildren<SpriteRenderer>().sprite;

        _arrowGO.transform.position = (Vector2)transform.position + _direction * 0.5f;

        var angle = Vector3.Angle(_arrowGO.transform.up, _direction);

        angle *= _direction.x != 0 ? _direction.x : _direction.y;

        _arrowGO.transform.Rotate(new(0, 0, angle));

        base.Awake();
    }

    protected override void Update()
    {
        base.Update();

        if (_timer >= cooldown)
        {
            Fire1(_direction);
            _timer = 0f;
        }
    }
}
