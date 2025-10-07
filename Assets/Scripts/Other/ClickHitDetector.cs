using System;
using UnityEngine;

public class ClickHitDetector : MonoBehaviour
{
    private void Awake()
    {
        EventProvider.Subscribe<IClickHitEvent>(OnHitAny);
    }

    private void OnDestroy()
    {
        EventProvider.Unsubscribe<IClickHitEvent>(OnHitAny);
    }

    private void OnHitAny(IClickHitEvent @event)
    {
        var collider = @event.FirstHit.collider;

        if (collider == null)
            return;

        IInteractable interact = collider.GetComponent<IInteractable>();

        if (@event.AllHits.Length == 1)
        {
            interact?.Interact();
            return;
        }

        foreach (var hit in @event.AllHits)
        {
            interact = hit.collider.GetComponent<IInteractable>();

            interact?.Interact();
        }
    }
}

public interface IInteractable
{
    void Interact();
}
