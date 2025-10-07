using System.Collections.Generic;
using UnityEngine;

public interface IBulletConfig
{
    float BulletSpeed { get; }
    GameObject BulletGO { get; }
    int MaxBullets { get; }
    List<GameObject> Bullets { get; }
    Transform BulletStartPos { get; }
}
