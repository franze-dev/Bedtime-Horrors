using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ClickHitDetector : MonoBehaviour
{
    private void Awake()
    {
        EventProvider.Subscribe<IClickEvent>(OnHitAny);
    }

    private void OnDestroy()
    {
        EventProvider.Unsubscribe<IClickEvent>(OnHitAny);
    }

    private void OnHitAny(IClickEvent @event)
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

public static class HoverChecker
{
    private static PauseController _pauseController;
    private static TutorialManager _tutorialManager;

    public static bool CheckHover<T>(out T anyObj, bool checkIfHoverAvailable = true)
    {
        anyObj = default;

        if (checkIfHoverAvailable)
        {
            if (!CanCheckHover())
                return false;
        }

        var screenPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        var hits = Physics2D.RaycastAll(screenPos, Vector2.zero);

        foreach (var hit in hits)
        {
            if (hit.collider == null) continue;

            if (hit.collider.gameObject.TryGetComponent<T>(out anyObj))
            {
                return true;
            }
        }

        return false;
    }

    public static bool CheckHover(Collider2D collider, bool checkIfHoverAvailable = true)
    {
        if (checkIfHoverAvailable)
        {
            if (!CanCheckHover())
                return false;
        }

        var screenPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        var hits = Physics2D.RaycastAll(screenPos, Vector2.zero);

        foreach (var hit in hits)
        {
            if (hit.collider == collider)
                return true;
        }

        return false;
    }

    public static bool CheckHoverNoRay(Bounds bounds, bool checkIfHoverAvailable = true)
    {
        if (checkIfHoverAvailable)
        {
            if (!CanCheckHover())
                return false;
        }

        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
        worldMousePos.z = 0;
        worldMousePos.z = bounds.center.z;

        return bounds.Contains(worldMousePos);
    }

    public static bool CanCheckHover()
    {
        if (!_pauseController)
            ServiceProvider.TryGetService(out _pauseController);

        if (!_tutorialManager)
            ServiceProvider.TryGetService(out _tutorialManager);

        if ((_pauseController && _pauseController.IsPaused) ||
           (_tutorialManager && !_tutorialManager.IsClickAllowed))
            return false;

        return true;
    }

    public static void Reset()
    {
        _pauseController = null;
        _tutorialManager = null;
    }
}