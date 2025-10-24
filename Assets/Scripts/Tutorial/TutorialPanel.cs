using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPanel : MonoBehaviour
{
    [SerializeField] private GameObject _handGO;
    [SerializeField] private GameObject _focusBackground;
    [SerializeField] private GameObject _focusPointsGO;

    private void OnEnable()
    {
        ShowHand(true);

        ShowFocus(true);
    }

    private void OnDisable()
    {
        ShowHand(false);

        ShowFocus(false);
    }

    private void ShowFocus(bool isFocus)
    {
        if (_focusBackground == null)
            return;

        _focusBackground.SetActive(isFocus);

        if (_focusPointsGO == null)
            return;

        _focusPointsGO.SetActive(isFocus);
    }

    public void ShowHand(bool show)
    {
        if (_handGO == null)
            return;

        _handGO.SetActive(show);
    }
}
