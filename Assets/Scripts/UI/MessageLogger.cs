using System;
using UnityEngine;

public class MessageLogger : MonoBehaviour
{
    [SerializeField] private StatTextUpdater _statTextUpdater;

    private void Awake()
    {
        if (_statTextUpdater == null)
            _statTextUpdater = GetComponent<StatTextUpdater>();

        if (_statTextUpdater == null)
            Debug.LogError("StatTextUpdater component not found in " + gameObject.name);

        EventProvider.Subscribe<ILogMessageEvent>(OnLogMessage);

        _statTextUpdater.UpdateText("");
    }

    private void OnDestroy()
    {
        EventProvider.Unsubscribe<ILogMessageEvent>(OnLogMessage);
    }

    private void OnLogMessage(ILogMessageEvent @event)
    {
        if (_statTextUpdater == null)
        {
            Debug.LogWarning("Stat text updater == null");
            return;
        }
        _statTextUpdater.UpdateText(@event.Message);

        StartCoroutine(_statTextUpdater.UpdateTextAfterDelay("", 2f));
    }
}
