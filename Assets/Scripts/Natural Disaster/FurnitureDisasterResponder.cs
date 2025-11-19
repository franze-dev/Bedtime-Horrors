using UnityEngine;
using DragonBones;
using System;

public class FurnitureDisasterResponder : MonoBehaviour
{
    [SerializeField] private UnityArmatureComponent _armature;
    [SerializeField] private DisasterReactionProfile _reactionProfile;

    private bool _disasterActive = false;

    private void Awake()
    {
        EventProvider.Subscribe<IOnDisasterStartEvent>(OnStart);
        EventProvider.Subscribe<IOnDisasterLoopEvent>(OnLoop);
        EventProvider.Subscribe<IOnDisasterEndEvent>(OnEnd);
        EventProvider.Subscribe<IOnNoDisasterEvent>(OnNoDisaster);
    }

    private void OnDestroy()
    {
        EventProvider.Unsubscribe<IOnDisasterStartEvent>(OnStart);
        EventProvider.Unsubscribe<IOnDisasterLoopEvent>(OnLoop);
        EventProvider.Unsubscribe<IOnDisasterEndEvent>(OnEnd);
        EventProvider.Unsubscribe<IOnNoDisasterEvent>(OnNoDisaster);
    }

    private void OnStart(IOnDisasterStartEvent evt)
    {
        _disasterActive = true;
        React(evt.Disaster, DisasterPhase.Start);
    }

    private void OnLoop(IOnDisasterLoopEvent evt)
    {
        React(evt.Disaster, DisasterPhase.Loop);
    }

    private void OnEnd(IOnDisasterEndEvent evt)
    {
        React(evt.Disaster, DisasterPhase.End);
    }

    private void OnNoDisaster(IOnNoDisasterEvent evt)
    {
        _disasterActive = false;

        if (_reactionProfile.Idle != null && !string.IsNullOrEmpty(_reactionProfile.Idle.AnimationName))
        {
            _armature.animation.Play(
                _reactionProfile.Idle.AnimationName,
                _reactionProfile.Idle.PlayTimes
            );
        }
    }

    private void React(NaturalDisaster disaster, DisasterPhase phase)
    {
        if (_reactionProfile == null || _reactionProfile.Reactions == null)
            return;

        _armature.gameObject.SetActive(true);
        foreach (var r in _reactionProfile.Reactions)
        {
            if (r == null || r.Disaster == null)
                continue;

            if (r.Disaster == disaster && r.Phase == phase)
            {
                if (!string.IsNullOrEmpty(r.AnimationName))
                    _armature.animation.Play(r.AnimationName, r.PlayTimes);

                if (r.DisableOnEnd && phase == DisasterPhase.End)
                {
                    _armature.animation.Stop();
                    _armature.gameObject.SetActive(false);
                }
            }
        }
    }

}
