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
    [SerializeField] private GameObject _photoParentGO;
    private List<GameObject> _diaryPhotos;

    private void Awake()
    {
        if (_diaryPages == null || _diaryPages.Count == 0)
        {
            Debug.LogError("No diary pages assigned on Diary Pages Manager");
            return;
        }

        _currentPageId = -1;

        _diaryPhotos = new List<GameObject>();

        foreach (var page in _diaryPages)
        {
            GameObject instance = Instantiate(page.photoPrefab, _photoParentGO.transform);
            instance.SetActive(false);
            _diaryPhotos.Add(instance);
        }

        ActivateNextPage();
    }

    private void UpdatePage(int pageId, int previousPage)
    {
        _titleMesh.text = _diaryPages[pageId].title;
        _storyMesh.text = _diaryPages[pageId].story;
        _tutorialMesh.text = _diaryPages[pageId].tutorial;
        if (previousPage >= 0)
            _diaryPhotos[previousPage].SetActive(false);
        _diaryPhotos[pageId].SetActive(true);
    }

    public void ActivateNextPage()
    {
        if (_currentPageId + 1 >= _diaryPages.Count)
            return;

        UpdatePage(_currentPageId + 1, _currentPageId);
        _currentPageId++;
    }

    public void ActivatePreviousPage()
    {
        if (_currentPageId <= 0)
            return;

        UpdatePage(_currentPageId - 1, _currentPageId);
        _currentPageId--;
    }
}
