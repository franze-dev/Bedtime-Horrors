using DragonBones;
using UnityEngine;

public class MyAnimator : MonoBehaviour
{
    [SerializeField] private UnityArmatureComponent _armatureComponent;
    public UnityArmatureComponent ArmatureComponent => _armatureComponent;

    private MyAnimationStates _currentState;

    public void Play(MyAnimationStates state, int playTimes = 0)
    {
        if (_currentState == state)
            return;

        _currentState = state;
        string animationName = state.ToString().ToUpper();

        if (AnimationExists(animationName))
            _armatureComponent.armature.animation.Play(animationName, playTimes);
    }

    public float GetAnimationDuration(MyAnimationStates state)
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

    public MyAnimationStates GetCurrentState() => _currentState;
}


public enum MyAnimationStates
{
    Idle,
    Walk,
    Attack,
    Death
}

