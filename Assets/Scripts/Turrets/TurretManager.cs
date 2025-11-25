using System;
using System.Collections.Generic;
using UnityEngine;

public class TurretManager : MonoBehaviour
{
    [SerializeField] private List<TurretSpawner> _turretSpawners;
    [SerializeField] private List<GameObject> _turretPrefabs;
    [SerializeField] private List<Turret> _activeTurrets;
    private bool _isFirstSpawn = true;
    public List<Turret> ActiveTurrets => _activeTurrets;

    private void Awake()
    {
        foreach (var spawner in _turretSpawners)
            spawner.SetTurretPrefabs(_turretPrefabs);

        EventProvider.Subscribe<ITurretSpawnEvent>(OnTurretSpawn);
        EventProvider.Subscribe<ITurretDestroyEvent>(OnTurretDestroy);
    }

    private void OnDestroy()
    {
        EventProvider.Unsubscribe<ITurretSpawnEvent>(OnTurretSpawn);
        EventProvider.Unsubscribe<ITurretDestroyEvent>(OnTurretDestroy);
    }

    private void OnTurretDestroy(ITurretDestroyEvent @event)
    {
        if (!@event.Turret)
            return;

        var turret = @event.Turret.GetComponent<Turret>();
        turret.Animator.Play(MyAnimationStates.Death, 1);

        AkUnitySoundEngine.PostEvent("Tower_Destroy", WwiseAudioHelper.DisasterSoundEmitter);

        _activeTurrets.Remove(@event.Turret.GetComponent<Turret>());
        Destroy(@event.Turret, turret.Animator.GetAnimationDuration(MyAnimationStates.Death));
    }

    private void OnTurretSpawn(ITurretSpawnEvent @event)
    {
        _activeTurrets.Add(@event.Turret.GetComponent<Turret>());
        if (_isFirstSpawn)
        {
            EventTriggerer.Trigger<IFirstTurretSpawnEvent>(new FirstTurretSpawnEvent());
            _isFirstSpawn = false;
        }
        AkUnitySoundEngine.PostEvent("Tower_Place", gameObject);
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < _turretSpawners.Count; i++)
        {
            Gizmos.color = Color.yellow;
            Bounds bounds = _turretSpawners[i].GetComponentInChildren<SpriteRenderer>().bounds;
            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }
    }
}
