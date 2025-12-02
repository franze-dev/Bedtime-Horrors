using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpeedMod", menuName = "ScriptableObjects/NaturalDisasters/SpeedMod")]
public class SpeedMod : NaturalDisaster, IDisasterUpdate
{
    [SerializeField] private float speedMultiplier = 1.5f;
    [SerializeField] private float _duration = 10f;
    private List<Enemy> _affectedEnemies;    

    public override void Init()
    {
        Duration = _duration;

        _affectedEnemies = new List<Enemy>();

        EventProvider.Subscribe<IEnemyCreateEvent>(OnEnemyCreate);
    }

    private void OnEnemyCreate(IEnemyCreateEvent @event)
    {
        var enemy = @event.Enemy;

        if (!_affectedEnemies.Contains(enemy))
            _affectedEnemies.Add(enemy);
    }

    public override void StartDisaster()
    {
        MultiplySpeed();
        AnimationLogic?.Play(AnimationData);
    }

    public override void EndDisaster()
    {
        ResetSpeed();
        AnimationLogic?.Stop();
    }

    private void MultiplySpeed()
    {
        Debug.Log("Speed Multiply");
        foreach (var enemy in _affectedEnemies)
            enemy.MultiplySpeed(speedMultiplier);
    }

    private void ResetSpeed()
    {
        Debug.Log("Speed Reset");
        foreach (var enemy in _affectedEnemies)
            enemy.ResetSpeed();
    }

    public void OnDestroy()
    {
        EventProvider.Unsubscribe<IEnemyCreateEvent>(OnEnemyCreate);
    }

    public void UpdateDisaster()
    {
        (AnimationLogic as IDisasterAnimationLoop)?.Loop();
    }
}