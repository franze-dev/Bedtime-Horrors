using UnityEngine;
using DragonBones;
using System;

public class FurnitureAnimationTriggerer : MonoBehaviour
{
    enum DisasterState
    {
        OnDisasterStart,
        OnDisasterLoop,
        OnDisasterEnd
    }

    [SerializeField] private UnityArmatureComponent _armature;
    [SerializeField] private string _animationName;
    [SerializeField] private int _playTimes;
    [SerializeField] private bool _isSpecialEffect;

    [SerializeField] NaturalDisaster _disasterToReact;
    [SerializeField] DisasterState _state;

    private bool HasSingleAnimation => _armature.animation.animations.Count == 1;
    private bool IsAnimationPlaying => _armature.animation.isPlaying;

    private void Awake()
    {
        if (_state == DisasterState.OnDisasterStart)
            EventProvider.Subscribe<IOnDisasterStartEvent>(PlayAnimation);

        if (_state == DisasterState.OnDisasterEnd || _isSpecialEffect)
            EventProvider.Subscribe<IOnDisasterEndEvent>(StopAnimation);

        if (_state == DisasterState.OnDisasterLoop)
            EventProvider.Subscribe<IOnDisasterLoopEvent>(LoopAnimation);
    }

    private void OnDestroy()
    {
        if (_state == DisasterState.OnDisasterStart)
            EventProvider.Unsubscribe<IOnDisasterStartEvent>(PlayAnimation);

        if (_state == DisasterState.OnDisasterEnd || _isSpecialEffect)
            EventProvider.Unsubscribe<IOnDisasterEndEvent>(StopAnimation);

        if (_state == DisasterState.OnDisasterLoop)
            EventProvider.Unsubscribe<IOnDisasterLoopEvent>(LoopAnimation);
    }

    private void PlayAnimation(IOnDisasterStartEvent @event)
    {
        if (@event.Disaster == _disasterToReact)
        {
            _armature.gameObject.SetActive(true);
            _armature.animation.Play(_animationName, _playTimes);
        }
    }

    private void StopAnimation(IOnDisasterEndEvent @event)
    {
        if (@event.Disaster == _disasterToReact)
        {
            if (!HasSingleAnimation)
                _armature.animation.Play(_animationName, _playTimes);
            else
                _armature.animation.Stop();

            if (_isSpecialEffect)
            {
                _armature.gameObject.SetActive(false);
                _armature.animation.Stop();
            }
        }
    }

    private void LoopAnimation(IOnDisasterLoopEvent @event)
    {
        if (@event.Disaster == _disasterToReact && !IsAnimationPlaying)
            _armature.animation.Play(_animationName);
    }
}
