using System;
using UnityEngine;

public class PanelEventTrigger : MonoBehaviour
{
    public TutorialPanel panel;
    public void TriggerEvent(bool isSequence = true)
    {
        EventTriggerer.Trigger<IContinuePanelsEvent>(new ContinuePanelsEvent(panel, isSequence));
    }
}