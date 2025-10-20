using System;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private List<TutorialPanel> _panels;
    private int _currentPanelIndex = -1;

    public int CurrentPanelIndex => _currentPanelIndex;

    private void Awake()
    {
        foreach (var panel in _panels)
        {
            panel?.gameObject.SetActive(false);
        }
        Time.timeScale = 0;

        ActivateNextPanel(_currentPanelIndex);

    }

    private void Start()
    {
        EventProvider.Subscribe<IContinuePanelsEvent>(OnContinuePanels);
    }

    private void OnContinuePanels(IContinuePanelsEvent @event)
    {
        var go = @event.Panel;

        var index = FindPanel(go);

        if (@event.IsSequencePanel)
        {
            if (index - 1 <= _currentPanelIndex && index != -1)
                ActivateNextPanel(_currentPanelIndex);
        }
        else if (index != -1)
            ActivatePanel(index);
    }

    private int FindPanel(TutorialPanel panel)
    {
        for (int i = 0; i < _panels.Count; i++)
            if (_panels[i] == panel)
                return i;

        return -1;
    }

    public void ActivateNextPanel(int currentIndex)
    {
        Time.timeScale = 1;

        if (currentIndex < _panels.Count)
        {
            if (currentIndex >= 0)
                _panels[currentIndex]?.gameObject.SetActive(false);

            if (_panels[currentIndex + 1] != null)
            {
                _panels[currentIndex + 1].gameObject.SetActive(true);
                _currentPanelIndex = currentIndex + 1;
                Time.timeScale = 0;
            }
            else
            {
                _currentPanelIndex = currentIndex + 1;
                Time.timeScale = 1;
            }
            //activar los eventos de este panel
        }
        else
            _panels[currentIndex].gameObject.SetActive(false);
    }

    private void ActivatePanel(int index)
    {
        if (_panels[index] == null)
            return;

        _panels[_currentPanelIndex]?.gameObject.SetActive(false);

        _panels[index].gameObject.SetActive(true);

        _currentPanelIndex = index;
    }
}
public class ContinuePanelsEvent : IContinuePanelsEvent
{
    private bool _isSequencePanel;
    private TutorialPanel _panel;

    public bool IsSequencePanel => _isSequencePanel;

    public TutorialPanel Panel => _panel;

    public GameObject TriggeredByGO => null;

    public ContinuePanelsEvent(TutorialPanel panelGO, bool IsSequence = true)
    {
        this._panel = panelGO;

        _isSequencePanel = IsSequence;
    }
}

public interface IContinuePanelsEvent : IEvent
{
    TutorialPanel Panel { get; }
    bool IsSequencePanel { get; }
}
