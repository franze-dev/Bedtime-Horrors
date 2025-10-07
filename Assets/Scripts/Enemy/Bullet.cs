using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Range(0.1f, 2f)] public float speed;
    public float range;
    public float damage;

    public Vector2 direction;
    public Vector2 nextDirection;
    public Vector2 initialPosition;

    public GameObject target;

    private void OnEnable()
    {
        initialPosition = transform.position;

        SetRotation(direction);
    }

    private void OnDisable()
    {
        direction = nextDirection;
    }

    private void Update()
    {
        Move();
        CheckLimitReached();
    }

    private void Move()
    {
        var currentTarget = target == null ? (Vector2)transform.position + direction : (Vector2)target.transform.position;

        transform.position = Vector2.MoveTowards(transform.position, currentTarget, speed * Time.deltaTime);

        if ((Vector2)transform.position == currentTarget)
            gameObject.SetActive(false);
    }

    private void CheckLimitReached()
    {
        float distanceTraveled = Vector2.Distance(transform.position, initialPosition);
        if (distanceTraveled >= range)
            this.gameObject.SetActive(false);
    }

    public void ResetBullet()
    {
        transform.position = initialPosition;
        direction = nextDirection;
        this.gameObject.SetActive(true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("bullet"))
            return;

        this.gameObject.SetActive(false);
    }

    public void SetRotation(Vector2 newDir)
    {
        transform.rotation = Quaternion.identity;

        var angle = Vector3.Angle(transform.right, newDir);

        angle *= newDir.x != 0 ? newDir.x : newDir.y;

        transform.Rotate(new(0, 0, angle));
    }
}
