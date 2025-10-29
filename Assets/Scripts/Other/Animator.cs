using DragonBones;
using UnityEngine;

public class Animator : MonoBehaviour
{
    [SerializeField] private UnityArmatureComponent _armatureComponent;
    public UnityArmatureComponent ArmatureComponent => _armatureComponent;

    private AnimationState _currentState;

    public void Play(AnimationState state, int playTimes = 0)
    {
        if (_currentState == state)
            return;

        _currentState = state;
        string animationName = state.ToString().ToUpper();

        if (AnimationExists(animationName))
            _armatureComponent.armature.animation.Play(animationName, playTimes);
    }

    public float GetAnimationDuration(AnimationState state)
    {
        string name = state.ToString().ToUpper();

        if (AnimationExists(name))
            return _armatureComponent.armature.animation.animations[name].duration;

        return 0;
    }

    public bool AnimationExists(string animationName)
    {
        return (_armatureComponent.armature.animation.animations.ContainsKey(animationName));
    }

    public bool IsAnimationPlaying()
    {
        return _armatureComponent.armature.animation.isPlaying;
    }

    public AnimationState GetCurrentState() => _currentState;
}


public enum AnimationState
{
    Idle,
    Walk,
    Attack,
    Death
}

