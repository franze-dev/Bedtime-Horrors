using System.Collections.Generic;
using UnityEngine;

public class WaveCreateEvent : IWaveCreateEvent
{
    private List<GameObject> _enemies;
    private NaturalDisaster _disaster;

    public List<GameObject> Enemies => _enemies;
    public NaturalDisaster Disaster => _disaster;

    public GameObject TriggeredByGO => null;

    public WaveCreateEvent(List<GameObject> enemies, NaturalDisaster disaster)
    {
        _enemies = new List<GameObject>();

        if (enemies == null)
            Debug.LogWarning("WaveCreateEvent: enemies list is null!");

        _enemies = enemies;
        _disaster = disaster;
    }
}