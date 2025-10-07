using UnityEngine;

public abstract class NaturalDisaster : ScriptableObject
{
    public float Duration { get; protected set; }

    public abstract void Init();
    public abstract void StartDisaster();
    public abstract void EndDisaster();
}
