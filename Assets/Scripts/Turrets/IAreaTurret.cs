using System.Collections.Generic;
using UnityEngine;

public interface IAreaTurret
{
    void CollisionEnter(Collision2D collision);
    void CollisionExit(Collision2D collision);

    List<GameObject> EnemiesCollided { get; }

}

