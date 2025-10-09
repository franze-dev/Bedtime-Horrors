using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "SpeedMod", menuName = "ScriptableObjects/NaturalDisasters/SpeedMod")]
public class SpeedMod : NaturalDisaster
{
    [SerializeField] private float speedMultiplier = 1.5f;
    [SerializeField] private float _duration = 10f;
    [SerializeField] private List<Enemy> _affectedEnemies;
    [SerializeField] private string _messageStart = "Speed!";
    [SerializeField] private string _messageEnd = "Speed End!";

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
        EventTriggerer.Trigger<ILogMessageEvent>(new LogMessageEvent(_messageStart, null));

        MultiplySpeed();

    }

    public override void EndDisaster()
    {
        EventTriggerer.Trigger<ILogMessageEvent>(new LogMessageEvent(_messageEnd, null));

        ResetSpeed();
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
}