using DragonBones;
using UnityEngine;

public class CustomAnimationUpdater : MonoBehaviour
{
    private UnityArmatureComponent _armatureComponent;
    private WorldClock _customClock = new WorldClock();

    private void Start()
    {
        _armatureComponent = GetComponent<UnityArmatureComponent>();

        if (_armatureComponent == null)
        {
            Debug.LogError("No se encontró UnityArmatureComponent en el objeto.");
            return;
        }

        UnityFactory.factory.clock.Remove(_armatureComponent.armature);
        _customClock.Add(_armatureComponent.armature);
    }



    void Update()
    {
        _customClock.AdvanceTime(Time.unscaledDeltaTime);
    }
}
