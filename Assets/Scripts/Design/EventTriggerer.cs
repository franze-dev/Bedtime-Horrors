using System;

/// <summary>
/// Triggers events
/// </summary>
public static class EventTriggerer
{
    public static void Trigger<T>(T myEvent) where T : IEvent
    {
        if (EventProvider.EventListeners.TryGetValue(typeof(T), out var action))
            (action as Action<T>)?.Invoke(myEvent);
    }
}
