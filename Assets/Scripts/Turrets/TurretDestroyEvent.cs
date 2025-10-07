using UnityEngine;

public class TurretDestroyEvent : ITurretDestroyEvent
{
    public GameObject Turret { get; private set; }
    public GameObject TriggeredByGO => Turret;

    public TurretDestroyEvent(GameObject turret)
    {
        Turret = turret;
    }
}