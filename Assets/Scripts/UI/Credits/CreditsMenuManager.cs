using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class CreditsMenuManager : MonoBehaviour
{
    [SerializeField] private Image _creditsBg;
    [SerializeField] private GameObject _contentGO;
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private GameObject _creditsTextPrefab;
    [SerializeField] private GameObject _creditsMenuGO;
    private List<GameObject> _instantiatedCreditEntries = new List<GameObject>();
    private CreditsSectionInfo _currentlyOpenedSection = null;

    private void Awake()
    {
        if (_creditsBg != null)
            _creditsBg.gameObject.SetActive(false);
        else
        {
            Debug.LogError("No credits bg found");
            return;
        }

        EventProvider.Subscribe<IOpenCreditMenuEvent>(OnCreditMenuOpen);
        EventProvider.Subscribe<ICloseCreditMenuEvent>(OnCreditMenuClose);
    }

    private void OnCreditMenuClose(ICloseCreditMenuEvent @event)
    {
        _creditsMenuGO.SetActive(false);
    }

    private void OnCreditMenuOpen(IOpenCreditMenuEvent @event)
    {
        if (_currentlyOpenedSection == @event.CreditsToShow)
            return;
        else
            _currentlyOpenedSection = @event.CreditsToShow;

        foreach (var entry in _instantiatedCreditEntries)
        {
            Destroy(entry);
        }

        _creditsMenuGO.SetActive(true);

        SetBackground(@event.CreditsToShow);
        _creditsBg.gameObject.SetActive(true);

        _titleText.text = @event.CreditsToShow.Title;

        InstantiateCredits(@event);

    }

    private void InstantiateCredits(IOpenCreditMenuEvent @event)
    {
        foreach (var credit in @event.CreditsToShow.Credits)
        {
            GameObject creditEntryGO = Instantiate(_creditsTextPrefab, _contentGO.transform);
            var creditText = creditEntryGO.GetComponent<TextMeshProUGUI>();
            creditText.text = credit.Credit;

            InstantiateLinks(credit, creditEntryGO);

            _instantiatedCreditEntries.Add(creditEntryGO);
        }
    }

    private void InstantiateLinks(CreditInfo credit, GameObject creditEntryGO)
    {
        foreach (var link in credit.Link)
        {
            if (link != null && link != "")
            {
                GameObject creditLinkGO = Instantiate(_creditsTextPrefab, _contentGO.transform);
                var creditLinkText = creditLinkGO.GetComponent<TextMeshProUGUI>();
                creditLinkText.text = $"<link=\"{link}\"><color=blue><u>{link}</u></color></link>";
                var fontSize = creditLinkText.fontSize;
                fontSize -= 4;
                creditLinkText.fontSize = fontSize;
                var button = creditLinkGO.GetComponent<Button>();
                if (button == null)
                    button = creditLinkGO.AddComponent<Button>();

                if (button != null)
                {
                    button.onClick.AddListener(() =>
                    {
                        Application.OpenURL(link);
                    });
                }

                _instantiatedCreditEntries.Add(creditLinkGO);
            }
        }
    }

    void SetBackground(CreditsSectionInfo creditInfo)
    {
        _creditsBg.sprite = creditInfo.Background;
        _creditsBg.SetNativeSize();
    }
}

public interface IOpenCreditMenuEvent : IEvent
{
    CreditsSectionInfo CreditsToShow { get; }
}

public class OpenCreditMenuEvent : IOpenCreditMenuEvent
{
    public GameObject TriggeredByGO => null;
    private CreditsSectionInfo _creditsToShow;
    public CreditsSectionInfo CreditsToShow => _creditsToShow;

    public OpenCreditMenuEvent(CreditsSectionInfo info)
    {
        _creditsToShow = info;
    }
}

public interface ICloseCreditMenuEvent : IEvent
{
}

public class CloseCreditMenuEvent : ICloseCreditMenuEvent
{
    public GameObject TriggeredByGO => null;
}
