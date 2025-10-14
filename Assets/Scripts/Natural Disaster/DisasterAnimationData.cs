using DragonBones;
using UnityEngine;

[CreateAssetMenu(fileName = "DisasterAnimationData", menuName = "ScriptableObjects/NaturalDisasters/DisasterAnimationData")]
public class DisasterAnimationData : ScriptableObject
{
    public GameObject animationPrefab;
}

public interface IDisasterAnimationLoop
{
    void Loop();
}

public interface IDisasterAnimation
{
    GameObject AnimationInstance { get; }
    UnityArmatureComponent Armature { get; }

    public void Play(DisasterAnimationData animation);

    public void Stop();
}

public abstract class DisasterAnimation : ScriptableObject, IDisasterAnimation
{
    public GameObject AnimationInstance { get; protected set; }
    public UnityArmatureComponent Armature { get; protected set; }

    public abstract void Play(DisasterAnimationData animation);

    public abstract void Stop();
}