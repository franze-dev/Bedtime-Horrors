using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiaryPagesManager : MonoBehaviour
{
    [SerializeField] private List<DiaryPage> _diaryPages;
    [SerializeField] private int _currentPageId;
    [SerializeField] private TextMeshProUGUI _titleMesh;
    [SerializeField] private TextMeshProUGUI _storyMesh;
    [SerializeField] private TextMeshProUGUI _tutorialMesh;
    [SerializeField] private Image _photoImage1;
    [SerializeField] private Image _photoImage2;

    private void Awake()
    {
        if (_diaryPages == null || _diaryPages.Count == 0)
        {
            Debug.LogError("No diary pages assigned on Diary Pages Manager");
            return;
        }

        _currentPageId = -1;

        ActivateNextPage();
    }

    private void UpdatePage(int pageId)
    {
        _titleMesh.text = _diaryPages[pageId].title;
        _storyMesh.text = _diaryPages[pageId].story;
        _tutorialMesh.text = _diaryPages[pageId].tutorial;
        _photoImage1.sprite = _diaryPages[pageId].photo;
        _photoImage2.sprite = _diaryPages[pageId].photo2;
    }

    public void ActivateNextPage()
    {
        if (_currentPageId + 1 >= _diaryPages.Count)
            return;

        UpdatePage(_currentPageId + 1);
        _currentPageId++;
    }

    public void ActivatePreviousPage()
    {
        if (_currentPageId <= 0)
            return;

        UpdatePage(_currentPageId - 1);
        _currentPageId--;
    }
}
