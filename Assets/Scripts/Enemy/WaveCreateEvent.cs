using System.Collections.Generic;
using UnityEngine;

public class WaveCreateEvent : IWaveCreateEvent
{
    private List<GameObject> _enemies;
    private NaturalDisaster _disaster;
    private GameObject _triggeredByGO;

    public List<GameObject> Enemies => _enemies;
    public NaturalDisaster Disaster => _disaster;

    public GameObject TriggeredByGO => _triggeredByGO;

    public WaveCreateEvent(List<GameObject> enemies, NaturalDisaster disaster, GameObject triggeredByGO)
    {
        _enemies = new List<GameObject>();

        if (enemies == null)
            Debug.LogWarning("WaveCreateEvent: enemies list is null!");

        _enemies = enemies;
        _triggeredByGO = triggeredByGO;
        _disaster = disaster;
    }
}