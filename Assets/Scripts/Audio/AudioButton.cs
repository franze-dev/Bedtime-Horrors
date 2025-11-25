using UnityEngine;

public class AudioButton : MonoBehaviour
{
    public void PlayNormalSound()
    {
        AkUnitySoundEngine.PostEvent("UI_Button_Normal", gameObject);
    }

    public void PlaySpecialSound()
    {
        AkUnitySoundEngine.PostEvent("UI_Button_Special", gameObject);
    }

    public void PlayDiaryPage()
    {
        AkUnitySoundEngine.PostEvent("UI_DiaryPage", gameObject);
    }

    public void PlayTransitionToLevel()
    {
        //AkUnitySoundEngine.PostEvent("TransitionTo_Level", gameObject);
        AkUnitySoundEngine.PostEvent("Ambient_Stop", gameObject);
        AkUnitySoundEngine.PostEvent("Disaster_Stop", gameObject);
    }

    public void PlayTransitionToMenu()
    {
        //AkUnitySoundEngine.PostEvent("TransitionTo_Menu", gameObject);
        AkUnitySoundEngine.PostEvent("Ambient_Stop", gameObject);
        AkUnitySoundEngine.PostEvent("Disaster_Stop", gameObject);
    }
}
