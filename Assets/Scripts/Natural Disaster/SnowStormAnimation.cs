using DragonBones;
using UnityEngine;

[CreateAssetMenu(fileName = "SnowStormAnimation", menuName = "ScriptableObjects/NaturalDisasters/Animations/SnowStormAnimation")]
public class SnowStormAnimation : DisasterAnimation, IDisasterAnimationLoop
{
    [SerializeField] private string _loopAnimationName = "Invierno_Loop";
    [SerializeField] private string _endAnimationName = "Invierno_End";

    public void Loop()
    {
        if (AnimationInstance != null)
        {
            if (Armature.animation.lastAnimationName == Armature.animation.animationNames[0])
            {
                bool firstAnimationCompleted = Armature.animation.isCompleted;
                if (firstAnimationCompleted)
                    Armature.animation.Play(_loopAnimationName);
            }
        }
    }

    public override void Play(DisasterAnimationData animation)
    {
        AkUnitySoundEngine.SetSwitch("Disaster_Type", "Blizzard", WwiseAudioHelper.DisasterSoundEmitter);
        AkUnitySoundEngine.PostEvent("Disaster_Start", WwiseAudioHelper.DisasterSoundEmitter);

        if (animation == null || animation.animationPrefab == null)
            return;

        AnimationInstance = Instantiate(animation.animationPrefab);
        Armature = AnimationInstance.GetComponent<UnityArmatureComponent>();
    }
    public override void Stop()
    {
        AkUnitySoundEngine.SetSwitch("Disaster_Type", "Blizzard", WwiseAudioHelper.DisasterSoundEmitter);
        AkUnitySoundEngine.PostEvent("Disaster_End", WwiseAudioHelper.DisasterSoundEmitter);
        
        if (AnimationInstance != null)
        {
            Armature = AnimationInstance.GetComponent<UnityArmatureComponent>();
            Armature.animation.Play(_endAnimationName, 1);
            float destroyTime = Armature.armature.animation.animations[_endAnimationName].duration;
            Destroy(AnimationInstance, destroyTime);
        }
    }
}
