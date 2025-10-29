using UnityEngine;
using UnityEngine.InputSystem;

public class DragDetector : MonoBehaviour
{
}


public interface IDragEvent : IEvent
{
    Vector2 MouseWorldPos { get; }
}


public class DragEvent : IDragEvent
{
    public GameObject TriggeredByGO => null;
    public Vector2 MouseWorldPos { get; }

    public DragEvent(Vector2 pos)
    {
        MouseWorldPos = pos;
    }
}
