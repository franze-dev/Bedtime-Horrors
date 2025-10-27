using UnityEngine;

public class LevelUpButton : MonoBehaviour
{
    [SerializeField] Turret _turret;

    public void LevelUp()
    {
        _turret.Upgrade();
        EventTriggerer.Trigger<ILevelUpEvent>(new LevelUpEvent());
    }
}

public interface ILevelUpEvent : IEvent
{

}

public class LevelUpEvent : ILevelUpEvent
{
    public GameObject TriggeredByGO => null;
}