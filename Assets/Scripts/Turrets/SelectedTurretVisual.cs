using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SelectedTurretVisual : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _turretNameMesh;
    [SerializeField] List<TextMeshProUGUI> _turretStatsMeshes;
    [SerializeField] GameObject _textsContainer;
    [Header("Open/Close visual")]
    [SerializeField] GameObject _closeButton;
    [SerializeField] GameObject _openButton;
    private bool _statsSet = false;
    private TutorialManager _tutorialManager;

    private void Awake()
    {
        ServiceProvider.SetService(this, true);
        Disable();
    }

    private void Start()
    {
        ServiceProvider.TryGetService(out _tutorialManager);
    }

    private void OnDestroy()
    {
        ServiceProvider.SetService<SelectedTurretVisual>(null);
    }

    public void Enable()
    {
        if (_tutorialManager != null && _tutorialManager.IsTutorialRunning)
            return;

        _textsContainer?.SetActive(true);
        _openButton?.SetActive(false);
        _closeButton?.SetActive(true);

        AkUnitySoundEngine.PostEvent("UI_Button_Normal", gameObject);
    }

    public void Disable()
    {
        if (_tutorialManager != null && _tutorialManager.IsTutorialRunning)
            return;

        _textsContainer?.SetActive(false);
        _openButton?.SetActive(true);
        _closeButton?.SetActive(false);

        AkUnitySoundEngine.PostEvent("UI_Button_Normal", gameObject);
    }

    public void SetStats(TurretStats currentStats, TurretStats nextStats, string turretName)
    {
        if (currentStats == null || nextStats == null)
            return;

        _turretNameMesh.text = turretName;

        if (_turretStatsMeshes != null && _turretStatsMeshes.Count > 0)
        {
            SetupText(_turretStatsMeshes[0], currentStats.Damage, nextStats != null ? nextStats.Damage : 0, "Damage");
            SetupText(_turretStatsMeshes[1], currentStats.Cooldown, nextStats != null ? nextStats.Cooldown : 0, "Cooldown");
            SetupText(_turretStatsMeshes[2], currentStats.Range, nextStats != null ? nextStats.Range : 0, "Range");

            _statsSet = true;
        }
    }

    private void SetupText(TextMeshProUGUI statMesh, float currentStat, float nextStat, string statName)
    {
        statMesh.text = statName + ": " + currentStat;

        if (nextStat > 0)
            statMesh.text += " - " + nextStat;
    }
}