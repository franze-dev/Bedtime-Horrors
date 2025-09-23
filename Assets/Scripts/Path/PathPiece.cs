using System;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

[ExecuteInEditMode]
[RequireComponent(typeof(SpriteRenderer))]
public class PathPiece : MonoBehaviour
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    [SerializeField] private PathSettings _pathInfo;
    public Direction SpawnDirection = Direction.Right;
    [SerializeField] private TileSet _tileSet;
    private Direction _previousDir = Direction.Right;
    private SpriteRenderer _spriteRenderer;

    private GameObject _pathPrefab;

    private void OnValidate()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        if (_pathInfo == null)
        {
            Debug.LogError("PathInfo is not assigned!");
            return;
        }

        _pathPrefab = _pathInfo.PathPrefab;

    }

    private Direction GetOpposite(Direction dir)
    {
        return dir switch
        {
            Direction.Up => Direction.Down,
            Direction.Down => Direction.Up,
            Direction.Left => Direction.Right,
            Direction.Right => Direction.Left,
            _ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
        };
    }

    private bool AreOpposite(Direction dirA, Direction dirB)
    {
        return GetOpposite(dirA) == dirB || GetOpposite(dirB) == dirA;
    }

    public void GeneratePath()
    {
        if (!_spriteRenderer)
            _spriteRenderer = GetComponent<SpriteRenderer>();
        else if (!_spriteRenderer)
            _spriteRenderer = gameObject.AddComponent<SpriteRenderer>();

        _spriteRenderer.sprite = _tileSet.GetTile(_previousDir);

        Sprite previous = _spriteRenderer.sprite;

        if (_pathInfo == null)
            Debug.LogError("PathInfo is not assigned!");

        if (_pathPrefab == null)
            _pathPrefab = _pathInfo.PathPrefab;

        if (_pathPrefab == null)
            Debug.LogError("PathPrefab is not assigned in PathInfo!");

        if (_previousDir != SpawnDirection && !AreOpposite(_previousDir, SpawnDirection))
        {
            EventTriggerer.Trigger<IPathDirectionChangeEvent>(new PathDirectionChangeEvent(this.gameObject));
            _spriteRenderer.sprite = _tileSet.GetTile(GetOpposite(_previousDir), SpawnDirection);
        }

        Vector2 spawnPos = CalculateSpawnPos(previous, _spriteRenderer.sprite);

        GameObject newPathPiece = Instantiate(_pathPrefab, spawnPos, Quaternion.identity, transform.parent);

        Selection.activeGameObject = newPathPiece;

        PathPiece pathPieceComponent = newPathPiece.GetComponent<PathPiece>();

        pathPieceComponent.SpawnDirection = SpawnDirection;

        pathPieceComponent._previousDir = SpawnDirection;

        Debug.Log("Previous Direction: " + _previousDir + ", New Direction: " + SpawnDirection);
    }

    private Vector2 CalculateSpawnPos(Sprite previous, Sprite next)
    {
        Vector2 sizeNext = next.bounds.size;
        Vector2 sizePrevious = previous.bounds.size;

        Vector2 commonSize = sizeNext;

        if (sizeNext != sizePrevious)
        {
            if (sizeNext.x != sizePrevious.x)
                commonSize.x = sizeNext.x > sizePrevious.x? sizePrevious.x : sizeNext.x;

            if (sizeNext.y != sizePrevious.y)
                commonSize.y = sizeNext.y > sizePrevious.y? sizePrevious.y : sizeNext.y;
        }

        Vector2 offSet = Vector2.zero;

        switch (SpawnDirection)
        {
            case Direction.Up:
                offSet = new(offSet.x, commonSize.y);
                break;
            case Direction.Down:
                offSet = new(offSet.x, -commonSize.y);
                break;
            case Direction.Left:
                offSet = new(-commonSize.x, offSet.y);
                break;
            case Direction.Right:
                offSet = new(commonSize.x, offSet.y);
                break;
            default:
                break;
        }

        Vector2 finalPos = (Vector2)transform.position + offSet;

        return finalPos;
    }
}
#endif