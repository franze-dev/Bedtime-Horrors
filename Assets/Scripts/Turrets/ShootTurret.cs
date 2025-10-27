using System.Collections.Generic;
using UnityEngine;
public abstract class ShootTurret : Turret, IBulletConfig
{
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private GameObject _bulletGO;
    [SerializeField] private int _maxBullets;
    [SerializeField] private List<GameObject> _bullets = new();
    [SerializeField] private Transform _bulletStartPosition;
    [SerializeField] private float _bulletRange;

    public float BulletSpeed { get => _bulletSpeed; set => _bulletSpeed = value; }
    public GameObject BulletGO { get => _bulletGO; set => _bulletGO = value; }
    public int MaxBullets { get => _maxBullets; set => _maxBullets = value; }
    public List<GameObject> Bullets { get => _bullets; set => _bullets = value; }
    public Transform BulletStartPos { get => _bulletStartPosition; set => _bulletStartPosition = value; }
    public float BulletRange { get => _bulletRange; set => _bulletRange = value; }

    public abstract void Fire();

    public virtual void AddNewBullet(Vector3 direction, float damage, GameObject target = null)
    {
        //Modificar para que los datos de la bala se pasen desde aca
        GameObject newBullet = Instantiate(BulletGO, BulletStartPos.position, Quaternion.identity);

        Bullet bulletComponent = newBullet.GetComponent<Bullet>();
        bulletComponent.direction = direction;
        bulletComponent.nextDirection = direction;
        bulletComponent.speed = BulletSpeed;
        bulletComponent.damage = damage;

        if (BulletRange == 0)
            Debug.LogWarning("Bullet range is 0");
        else
            bulletComponent.range = BulletRange;

        if (target != null)
            bulletComponent.target = target;

        Bullets.Add(newBullet);
    }

    public virtual void RetargetBullets(GameObject target = null)
    {
        for (int i = 0; i < Bullets.Count; i++)
            if (!Bullets[i].gameObject.activeSelf)
            {
                Bullets[i].GetComponent<Bullet>().ResetBullet();
                Bullets[i].GetComponent<Bullet>().target = target;
                return;
            }
    }

    public void Fire(Vector2 direction)
    {
        if (Bullets.Count < MaxBullets)
        {
            //Modificar para que los datos de la bala se pasen desde aca
            GameObject newBullet = Instantiate(BulletGO, BulletStartPos.position, Quaternion.identity);

            Bullet bulletComponent = newBullet.GetComponent<Bullet>();
            bulletComponent.direction = direction;
            bulletComponent.nextDirection = direction;
            bulletComponent.speed = BulletSpeed;
            bulletComponent.damage = damage;
            bulletComponent.range = range;

            bulletComponent.SetRotation(direction);

            Bullets.Add(newBullet);
        }
        else
        {
            for (int i = 0; i < Bullets.Count; i++)
                if (!Bullets[i].gameObject.activeSelf)
                {
                    Bullets[i].GetComponent<Bullet>().ResetBullet();
                    return;
                }
        }
    }

}
