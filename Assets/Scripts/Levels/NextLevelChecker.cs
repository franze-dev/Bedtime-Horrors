using UnityEngine;

public class NextLevelChecker : MonoBehaviour
{
    private void Start()
    {
        EventTriggerer.Trigger<ICheckNextLevelEvent>(new CheckNextLevelEvent());
    }
}