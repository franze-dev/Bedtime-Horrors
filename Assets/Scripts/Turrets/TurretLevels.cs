using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TurretLevels", menuName = "ScriptableObjects/TurretLevels")]
public class TurretLevels : ScriptableObject
{
    [SerializeField] private List<TurretStats> _levelsStats;
    [SerializeField] private int _startLevelId = 0;

    public int StartLevelId => _startLevelId;
    public int LastLevelId => _startLevelId + _levelsStats.Count-1;

    public int LevelCount => _levelsStats.Count;

    public TurretStats GetStats(int levelId)
    {
        if (levelId >= 0 && levelId < _levelsStats.Count)
            return _levelsStats[levelId];

        if (levelId >= _levelsStats.Count)
            return _levelsStats[LastLevelId];
        else
            return _levelsStats[_startLevelId];
    }
}