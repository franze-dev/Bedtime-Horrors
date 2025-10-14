using DragonBones;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "SpeedMod", menuName = "ScriptableObjects/NaturalDisasters/SpeedMod")]
public class SpeedMod : NaturalDisaster, IDisasterUpdate
{
    [SerializeField] private float speedMultiplier = 1.5f;
    [SerializeField] private float _duration = 10f;
    [SerializeField] private DisasterAnimation _animation;
    [SerializeField] private List<Enemy> _affectedEnemies;
    [SerializeField] private string _messageStart = "Speed!";
    [SerializeField] private string _messageEnd = "Speed End!";

    private GameObject _animationObject;
    private GameObject _animationObjectInstance;
    private UnityArmatureComponent _animationArmature;

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
        StartAnimation();
    }

    public override void EndDisaster()
    {
        EventTriggerer.Trigger<ILogMessageEvent>(new LogMessageEvent(_messageEnd, null));

        ResetSpeed();
        EndAnimation();
        //_affectedEnemies?.Clear();
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

    public override void StartAnimation()
    {
        if (DisasterAnimation != null)
        {
            _animationObject = DisasterAnimation.animationPrefab;
            _animationObjectInstance = Instantiate(_animationObject);
            _animationArmature = _animationObjectInstance.GetComponent<UnityArmatureComponent>();
            //animationArmature.animation.Play(animationArmature.animation.animationNames[0], 1);
            Debug.LogWarning("Playing animation");
        }
    }

    public override void EndAnimation()
    {
        if (_animationObjectInstance != null)
        {
            Debug.LogWarning("End animation entered");

            _animationArmature = _animationObjectInstance.GetComponent<UnityArmatureComponent>();
            _animationArmature.animation.Play("Inundacion_End", 1);
            float destroyTime = _animationArmature.armature.animation.animations["Inundacion_End"].duration;
            Destroy(_animationObjectInstance, destroyTime);
        }
    }

    public void UpdateDisaster()
    {
        if (_animationObjectInstance != null)
        {
            if (_animationArmature.animation.lastAnimationName == _animationArmature.animation.animationNames[0])
            {
                bool firstAnimationCompleted = _animationArmature.animation.isCompleted;
                if (firstAnimationCompleted)
                    _animationArmature.animation.Play("Inundacion_Loop");
            }
        }
    }
}