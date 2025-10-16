using System.Collections.Generic;
using UnityEngine;

public interface IWaveCreateEvent : IEvent
{
    public List<GameObject> Enemies { get; }
    public NaturalDisaster Disaster { get; }
}