using UnityEngine;

public interface ITurretDestroyEvent : IEvent
{
    GameObject Turret { get; }
}