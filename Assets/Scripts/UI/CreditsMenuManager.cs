using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CreditsMenuManager : MonoBehaviour
{
    [SerializeField] private Image _creditsBg;

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
    }

    private void OnCreditMenuOpen(IOpenCreditMenuEvent @event)
    {

    }

    void SetBackground(CreditsSectionInfo creditInfo)
    {
        _creditsBg.sprite = creditInfo.Background;
        _creditsBg.SetNativeSize();
    }
}

public interface IOpenCreditMenuEvent : IEvent
{
}

public class OpenCreditMenuEvent : IOpenCreditMenuEvent
{
    public GameObject TriggeredByGO => null;

}

public class CreditsMenuOpener
{
    [SerializeField] private CreditInfo _creditsToOpen;
}

public class CreditsSectionInfo : ScriptableObject
{
    [SerializeField] private string _title;
    [SerializeField] List<CreditInfo> _credits;
    [SerializeField] private Sprite _background;

    public Sprite Background => _background;
}

public class CreditInfo : ScriptableObject
{
    [SerializeField] private string _credit;
    [SerializeField] private string _link;
}