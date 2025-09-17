using System;
using System.Collections.Generic;
using UnityEngine;

public class TurretManager : MonoBehaviour
{
    [SerializeField] private List<TurretSpawner> _turretSpawners;
    [SerializeField] private List<GameObject> _turretPrefabs;
    [SerializeField] private List<GameObject> _activeTurrets;
    public List<GameObject> ActiveTurrets => _activeTurrets;

    private void Awake()
    {
        foreach (var spawner in _turretSpawners)
            spawner.SetTurretPrefabs(_turretPrefabs);

        EventProvider.Subscribe<ITurretSpawnEvent>(OnTurretSpawn);
        EventProvider.Subscribe<ITurretDestroyEvent>(OnTurretDestroy);
    }

    private void OnTurretDestroy(ITurretDestroyEvent @event)
    {
        if (!@event.Turret)
            return;

        _activeTurrets.Remove(@event.Turret);

        Destroy(@event.Turret);
    }

    private void OnTurretSpawn(ITurretSpawnEvent @event)
    {
        _activeTurrets.Add(@event.Turret);
    }
}
