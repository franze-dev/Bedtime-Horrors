using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelContainer", menuName = "ScriptableObjects/LevelContainer")]
public class LevelContainer : ScriptableObject
{
    public Level bootLevel;
    public List<Level> gameplayLevels;
    public Level menusLevel;
}
