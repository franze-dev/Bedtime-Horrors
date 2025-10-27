using DragonBones;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    [SerializeField] private UnityArmatureComponent _armatureComponent;
    public UnityArmatureComponent ArmatureComponent => _armatureComponent;

    private EnemyAnimationState _currentState;

    public void Play(EnemyAnimationState state, int playTimes = 0)
    {
        if (_currentState == state)
            return;

        _currentState = state;
        string animationName = state.ToString().ToUpper();

        if (AnimationExists(animationName))
            _armatureComponent.armature.animation.Play(animationName, playTimes);
    }

    public float GetAnimationDuration(EnemyAnimationState state)
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

    public EnemyAnimationState GetCurrentState() => _currentState;
}


public enum EnemyAnimationState
{
    Walk,
    Death
}

