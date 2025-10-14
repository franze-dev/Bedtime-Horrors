using System;
using UnityEngine;

public abstract class NaturalDisaster : ScriptableObject
{
    public float Duration { get; protected set; }
    [SerializeField] private DisasterAnimation _disasterAnimation;
    [HideInInspector] public DisasterAnimation DisasterAnimation { get => _disasterAnimation; protected set => _disasterAnimation = value; }

    public abstract void Init();
    public abstract void StartDisaster();
    public abstract void EndDisaster();
    public abstract void StartAnimation();
    public abstract void EndAnimation();
}
