using UnityEngine;

public class EnemyCreateEvent : IEnemyCreateEvent
{
    private Enemy _enemy;
    private GameObject _gameObject;
    public Enemy Enemy => _enemy;
    public GameObject TriggeredByGO => _gameObject;

    public EnemyCreateEvent(Enemy enemy)
    {
        _enemy = enemy;
        _gameObject = _enemy.gameObject;
    }
}
