using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private IMusicState _currentState;
    private IMusicState _previousState;

    private void Awake()
    {
        ServiceProvider.SetService(this);
        AkUnitySoundEngine.PostEvent("Music_Initialize", WwiseAudioHelper.GlobalSoundEmitter);
    }

    private void OnDestroy()
    {
        AkUnitySoundEngine.PostEvent("Music_Stop", WwiseAudioHelper.GlobalSoundEmitter);
    }

    public void ToState(IMusicState state)
    {
        state.Enter();
        _previousState = _currentState;
        _currentState = state;
    }
}

public interface IMusicState
{
    void Enter();
}

public class LevelMusicState : IMusicState
{
    private string _stage;

    public LevelMusicState(int level)
    {
        _stage = "Stage_" + level.ToString();
    }

    public void Enter()
    {
        AkUnitySoundEngine.SetState("Music_State", _stage);
    }
}

public class MenuMusicState : IMusicState
{
    public void Enter()
    {
        AkUnitySoundEngine.SetState("Music_State", "Menu");
    }
}

public class VictoryMusicState : IMusicState
{
    public void Enter()
    {
        AkUnitySoundEngine.SetState("Music_State", "Stage_Victory");
    }
}

public class DefeatMusicState : IMusicState
{
    public void Enter()
    {
        AkUnitySoundEngine.SetState("Music_State", "Stage_Defeat");
    }
}
