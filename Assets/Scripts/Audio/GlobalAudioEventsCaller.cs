using DragonBones;
using UnityEngine;

public static class GlobalAudioEventsCaller
{
    public static void StopGameplaySounds()
    {
        //AkUnitySoundEngine.PostEvent("TransitionTo_Level", gameObject);
        AkUnitySoundEngine.PostEvent("Ambient_Stop", WwiseAudioHelper.GlobalSoundEmitter);
        AkUnitySoundEngine.PostEvent("Disaster_Stop", WwiseAudioHelper.GlobalSoundEmitter);
    }
}
