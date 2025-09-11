using UnityEngine;

/// <summary>
/// Interface for all events
/// </summary>
public interface IEvent
{
    /// <summary>
    /// Saves the gameObject the event was triggered by
    /// </summary>
    GameObject TriggeredByGO { get; } 
}
