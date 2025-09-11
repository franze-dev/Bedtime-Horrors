using UnityEngine;

public class LogMessageEvent : ILogMessageEvent
{
    private GameObject _triggeredByGO;

    public string Message { get; private set; }
    public GameObject TriggeredByGO => _triggeredByGO;

    public LogMessageEvent(string message, GameObject gameObject)
    {
        Message = message;
        _triggeredByGO = gameObject;
    }
}