using UnityEngine;

public class FloatingText : MonoBehaviour
{
    private Rigidbody2D _rigidBody;
    private TextUpdater _text;

    [SerializeField] private float _initalVelocityY;
    [SerializeField] private float _rangeVelocityX;
    [SerializeField] private float _lifeTime;

    public float LifeTime => _lifeTime;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _text = GetComponentInChildren<TextUpdater>();
    }

    private void Start()
    {
        float initialVelocityX = Random.Range(-_rangeVelocityX, _rangeVelocityX);
        _rigidBody.AddForce(new Vector2(initialVelocityX, _initalVelocityY), ForceMode2D.Impulse);
        Destroy(gameObject, _lifeTime);
    }

    public void SetText(string text)
    {
        _text?.ChangeText(text);
    }
}
