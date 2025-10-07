using System;
using UnityEngine;

#if UNITY_EDITOR

[CreateAssetMenu(fileName = "TileSettings", menuName = "ScriptableObjects/Path/TileSettings")]
public class TileSettings : ScriptableObject
{
    [SerializeField] private Sprite tileSprite;
    [SerializeField] private PathPiece.Direction _startDir;
    [SerializeField] private PathPiece.Direction _endDir;
    public bool IsCorner => !IsVertical && !IsHorizontal;

    public Sprite TileSprite { get => tileSprite; set => tileSprite = value; }
    public PathPiece.Direction StartDir => _startDir;
    public PathPiece.Direction EndDir => _endDir;

    private bool IsVertical => 
        (StartDir == PathPiece.Direction.Up && EndDir == PathPiece.Direction.Down) || 
        (StartDir == PathPiece.Direction.Down && EndDir == PathPiece.Direction.Up);
    private bool IsHorizontal => 
        (StartDir == PathPiece.Direction.Left && EndDir == PathPiece.Direction.Right) || 
        (StartDir == PathPiece.Direction.Right && EndDir == PathPiece.Direction.Left);

    public bool ContainsDirs(PathPiece.Direction dirA, PathPiece.Direction dirB)
    {
        return (StartDir == dirA && EndDir == dirB) || 
               (StartDir == dirB && EndDir == dirA);
    }

    public bool ContainsDir(PathPiece.Direction dir)
    {
        return StartDir == dir || EndDir == dir;
    }
}

#endif