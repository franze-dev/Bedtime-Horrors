using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SelectedTurretVisual : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _turretNameMesh;
    [SerializeField] List<TextMeshProUGUI> _turretStatsMeshes;
    [SerializeField] GameObject _textsContainer;

    private void Awake()
    {
        ServiceProvider.SetService(this);
    }

    private void OnDestroy()
    {
        ServiceProvider.SetService<SelectedTurretVisual>(null);
    }

    public void Enable()
    {
        _textsContainer.SetActive(true);
    }

    public void Disable()
    {
        _textsContainer?.SetActive(false);
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
        }
    }

    private void SetupText(TextMeshProUGUI statMesh, float currentStat, float nextStat, string statName)
    {
        statMesh.text = statName + ": " + currentStat;

        if (nextStat > 0)
            statMesh.text += " - " + nextStat;
    }
}