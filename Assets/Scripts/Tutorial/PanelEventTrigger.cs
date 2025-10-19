using System;
using UnityEngine;

public class PanelEventTrigger : MonoBehaviour
{
    public GameObject panel;
    public void TriggerEvent(bool isSequence = true)
    {
        EventTriggerer.Trigger<IContinuePanelsEvent>(new ContinuePanelsEvent(panel, isSequence));
    }
}