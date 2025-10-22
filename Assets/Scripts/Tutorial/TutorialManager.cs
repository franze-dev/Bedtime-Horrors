using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private List<TutorialPanel> _panels = new();
    [SerializeField] private TutorialPanel _transitionPanel;
    private int _currentPanelIndex = -1;

    public int CurrentPanelIndex => _currentPanelIndex;

    private void Awake()
    {
        if (_panels == null || _panels.Count == 0)
        {
            Debug.LogError("No panels assigned to TutorialManager.");
            return;
        }

        foreach (var panel in _panels)
        {
            if (panel == null)
            {
                Debug.LogWarning("A panel in the TutorialManager list is null.");
                continue;
            }

            if (panel.gameObject == null)
            {
                Debug.LogWarning("A panel's GameObject in the TutorialManager list is null.");
                continue;
            }

            panel.gameObject.SetActive(false);
        }

        Time.timeScale = 0;

        if (_panels.Count > 0)
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

            if (_panels[currentIndex + 1] != null && _panels[currentIndex + 1] != _transitionPanel)
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
