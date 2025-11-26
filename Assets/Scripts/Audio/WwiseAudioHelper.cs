using UnityEngine;

public static class WwiseAudioHelper
{
    private static GameObject _disasterSoundEmitter;
    private static GameObject _globalSoundEmitter;

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

    public static GameObject GlobalSoundEmitter
    {
        get
        {
            if (_globalSoundEmitter == null)
            {
                _globalSoundEmitter = new GameObject("GlobalSoundEmitter");
                AkUnitySoundEngine.RegisterGameObj(_globalSoundEmitter);
                Object.DontDestroyOnLoad(_globalSoundEmitter);
            }
            return _globalSoundEmitter;
        }
    }
}