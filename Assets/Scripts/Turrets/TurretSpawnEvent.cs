using UnityEngine;

public class TurretSpawnEvent : ITurretSpawnEvent
{
    public GameObject Turret { get; private set; }
    public GameObject TriggeredByGO => Turret;

    public TurretSpawnEvent(GameObject turret)
    {
        Turret = turret;

    }
}
