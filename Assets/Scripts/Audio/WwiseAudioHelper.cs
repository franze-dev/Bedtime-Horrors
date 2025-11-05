using UnityEngine;

public static class WwiseAudioHelper
{
    private static GameObject _disasterSoundEmitter;

    public static GameObject DisasterSoundEmitter
    {
        get
        {
            if (_disasterSoundEmitter == null)
            {
                _disasterSoundEmitter = new GameObject("DisasterSoundEmitter");
                AkUnitySoundEngine.RegisterGameObj(_disasterSoundEmitter);
                Object.DontDestroyOnLoad(_disasterSoundEmitter);
            }
            return _disasterSoundEmitter;
        }
    }
}