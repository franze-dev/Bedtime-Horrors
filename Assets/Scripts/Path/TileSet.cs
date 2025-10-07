using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR

[CreateAssetMenu(fileName = "TileSet", menuName = "ScriptableObjects/Path/TileSet")]
public class TileSet : ScriptableObject
{
    [SerializeField] private List<TileSettings> _tiles;

    public Sprite GetTile(PathPiece.Direction dirA, PathPiece.Direction dirB)
    {
        foreach (var tile in _tiles)
            if (tile.ContainsDirs(dirA, dirB))
                return tile.TileSprite;

        Debug.LogWarning($"No tile found for directions {dirA} and {dirB}");
        return null;
    }

    public Sprite GetTile(PathPiece.Direction dir)
    {
        foreach (var tile in _tiles)
            if (!tile.IsCorner)
            {
                if (tile.ContainsDir(dir))
                    return tile.TileSprite;
            }
        Debug.LogWarning($"No tile found for direction {dir} that is straight");
        return null;
    }
}

#endif