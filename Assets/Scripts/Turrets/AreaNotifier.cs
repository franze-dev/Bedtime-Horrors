using UnityEngine;

public class AreaNotifier : MonoBehaviour
{
    private IAreaTurret _parentTurret;

    private void Awake()
    {
        _parentTurret = gameObject.GetComponentInParent<IAreaTurret>();
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
