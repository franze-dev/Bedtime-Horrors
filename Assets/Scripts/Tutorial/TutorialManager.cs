using Coffee.UIExtensions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private List<TutorialPanel> _panels = new();
    [SerializeField] private TutorialPanel _transitionPanel;
    [SerializeField] private GameObject _backgroundGO;
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private float _backgroundMaxAlpha = 225;
    private int _currentPanelIndex = -1;

    public int CurrentPanelIndex => _currentPanelIndex;

    private void Awake()
    {
        if (_panels == null || _panels.Count == 0)
        {
            Debug.LogError("No panels assigned to TutorialManager.");
            return;
        }

        if (_backgroundImage == null)
            Debug.LogError("No background image assigned to TutorialManager");

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

    private void OnDestroy()
    {
        EventProvider.Unsubscribe<IContinuePanelsEvent>(OnContinuePanels);
    }

    private void OnContinuePanels(IContinuePanelsEvent @event)
    {
        ActivateNextPanel(_currentPanelIndex);
    }

    public void ActivateNextPanel(int currentIndex)
    {
        Time.timeScale = 1;

        if (currentIndex < _panels.Count)
        {
            if (currentIndex >= 0)
                _panels[currentIndex]?.gameObject.SetActive(false);

            if (currentIndex + 1 < _panels.Count)
            {
                if (_panels[currentIndex + 1] != null && _panels[currentIndex + 1] != _transitionPanel)
                {
                    if (!_backgroundGO.activeSelf)
                        _backgroundGO.SetActive(true);

                    _panels[currentIndex + 1].gameObject.SetActive(true);

                    if (_panels[currentIndex + 1].HasFocusPoints())
                    {
                        if (_backgroundImage != null)
                        {
                            var screenColor = _backgroundImage.color;
                            float alpha = _backgroundMaxAlpha / 255;
                            screenColor.a = alpha;
                            _backgroundImage.color = screenColor;
                        }
                    }
                    else
                    {
                        if (_backgroundImage != null)
                        {
                            var screenColor = _backgroundImage.color;
                            screenColor.a = 0;
                            _backgroundImage.color = screenColor;
                        }
                    }

                    _currentPanelIndex = currentIndex + 1;
                    Time.timeScale = 0;

                }
                else
                {
                    if (_backgroundGO.activeSelf)
                        _backgroundGO.SetActive(false);

                    _currentPanelIndex = currentIndex + 1;
                    Time.timeScale = 1;
                }
            }
        }
        else
            _panels[currentIndex].gameObject.SetActive(false);
    }
}
public class ContinuePanelsEvent : IContinuePanelsEvent
{
    private bool _isSequencePanel;

    public bool IsSequencePanel => _isSequencePanel;

    public GameObject TriggeredByGO => null;

    public ContinuePanelsEvent(bool IsSequence = true)
    {
        _isSequencePanel = IsSequence;
    }
}

public interface IContinuePanelsEvent : IEvent
{
    bool IsSequencePanel { get; }
}
