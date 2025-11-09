using UnityEngine;

public abstract class NaturalDisaster : ScriptableObject
{
    public float Duration { get; protected set; }
    [SerializeField] private DisasterAnimationData _disasterAnimationData;
    [SerializeField] private DisasterAnimation _animationLogic;
    [SerializeField] private Sprite _icon;


    [HideInInspector] public DisasterAnimationData AnimationData { get => _disasterAnimationData; protected set => _disasterAnimationData = value; }
    [HideInInspector] public DisasterAnimation AnimationLogic { get => _animationLogic; protected set => _animationLogic = value; }
    [HideInInspector] public Sprite Icon { get => Icon; protected set => Icon = value; }

    public abstract void Init();
    public abstract void StartDisaster();
    public abstract void EndDisaster();
}