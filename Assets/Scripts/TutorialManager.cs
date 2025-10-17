using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _panels;
    private int _currentPanelIndex = -1;

    public int CurrentPanelIndex => _currentPanelIndex;

    private void Awake()
    {
        foreach (var panel in _panels)
        {
            panel.SetActive(false);
        }
        Time.timeScale = 0;

        ActivateNextPanel(_currentPanelIndex);
    }

    public void ActivateNextPanel(int currentIndex)
    {
        Time.timeScale = 1;

        if (currentIndex < _panels.Count)
        {
            if (currentIndex >= 0)
                _panels[currentIndex].SetActive(false);

            _panels[currentIndex + 1].SetActive(true);
            Time.timeScale = 0;
        }
        else
            _panels[currentIndex].SetActive(false);

        _currentPanelIndex = currentIndex + 1;

        Time.timeScale = 0;
    }
}
