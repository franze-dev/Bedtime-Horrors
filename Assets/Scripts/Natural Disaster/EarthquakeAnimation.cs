using DragonBones;
using UnityEngine;

[CreateAssetMenu(fileName = "EarthquakeAnimation", menuName = "ScriptableObjects/NaturalDisasters/Animations/EarthquakeAnimation")]
public class EarthquakeAnimation : DisasterAnimation
{
    public override void Play(DisasterAnimationData animation)
    {
        if (animation == null || animation.animationPrefab == null)
            return;

        AnimationInstance = Instantiate(animation.animationPrefab);
        Armature = AnimationInstance.GetComponent<UnityArmatureComponent>();

    }
    public override void Stop()
    {
        if (AnimationInstance != null)
            Destroy(AnimationInstance);
    }
}