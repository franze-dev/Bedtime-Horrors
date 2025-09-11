using UnityEngine;

public interface ITurretSpawnEvent : IEvent
{
    GameObject Turret { get; }
}
