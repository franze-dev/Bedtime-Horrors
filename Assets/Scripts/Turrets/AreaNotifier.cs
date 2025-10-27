using UnityEngine;

public class AreaNotifier : MonoBehaviour
{
    [SerializeField] private float _spriteScale = 2f;
    private IAreaTurret _parentTurret;
    private float _range;
    private CircleCollider2D _circleCollider;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _parentTurret = GetComponentInParent<IAreaTurret>();
        _circleCollider = GetComponent<CircleCollider2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();


    }

    public void SetRange(float range)
    {
        float scaledRange = range * _spriteScale;

        _spriteRenderer.transform.localScale = new(scaledRange, scaledRange, scaledRange);
        _circleCollider.radius = range;
        _range = range;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _parentTurret?.CollisionEnter(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        _parentTurret?.CollisionExit(collision);
    }
}
