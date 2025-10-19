using System;
using UnityEngine;

public class NextPanelButton : MonoBehaviour
{
    [SerializeField] private TutorialManager _tutorialManager;

    private void Awake()
    {
        if (_tutorialManager == null)
            _tutorialManager = GetComponentInParent<TutorialManager>();
    }

    public void GoToNextPanel()
    {
        _tutorialManager.ActivateNextPanel(_tutorialManager.CurrentPanelIndex);
    }
}