using UnityEngine;

public class AudioButton : MonoBehaviour
{
    public void PlayNormalSound()
    {
        AkUnitySoundEngine.PostEvent("UI_Button_Normal", WwiseAudioHelper.GlobalSoundEmitter);
    }

    public void PlaySpecialSound()
    {
        AkUnitySoundEngine.PostEvent("UI_Button_Special", WwiseAudioHelper.GlobalSoundEmitter);
    }

    public void PlayDiaryPage()
    {
        AkUnitySoundEngine.PostEvent("UI_DiaryPage", WwiseAudioHelper.GlobalSoundEmitter);
    }

    public void PlayTransitionToLevel()
    {
        GlobalAudioEventsCaller.StopGameplaySounds();
    }

    public void PlayTransitionToMenu()
    {
        GlobalAudioEventsCaller.StopGameplaySounds();
    }
}
