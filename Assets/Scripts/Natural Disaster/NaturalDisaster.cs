using UnityEngine;

public abstract class NaturalDisaster : ScriptableObject
{
    public float Duration { get; protected set; }
    [SerializeField] private DisasterAnimationData _disasterAnimationData;
    [SerializeField] private DisasterAnimation _animationLogic;
    [HideInInspector] public DisasterAnimationData AnimationData { get => _disasterAnimationData; protected set => _disasterAnimationData = value; }
    [HideInInspector] public DisasterAnimation AnimationLogic { get => _animationLogic; protected set => _animationLogic = value; }

    public abstract void Init();
    public abstract void StartDisaster();
    public abstract void EndDisaster();
}