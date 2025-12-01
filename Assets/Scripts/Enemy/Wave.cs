using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    public List<GameObject> _enemies = new();
    public float cooldownBetweenEnemies;
    public int currentEnemyIndex;
    public float timeToNextWave;

    [SerializeField] private NaturalDisaster _disaster;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private bool _waitForEnemiesToDie;

    public NaturalDisaster Disaster => _disaster;

    private void Start()
    {
        _disaster?.Init();
    }

    public void InitWave()
    {
        if (_disaster != null)
            EventTriggerer.Trigger<IStartFixedDisasterEvent>(new StartFixedDisasterEvent(_disaster));

        foreach (var enemy in _enemies)
        {
            var enemyComp = enemy.GetComponent<Enemy>();

            EventTriggerer.Trigger<IEnemyCreateEvent>(new EnemyCreateEvent(enemyComp));

            enemy.SetActive(false);
        }

        currentEnemyIndex = 0;
    }

    public void SpawnEnemy()
    {
        if (currentEnemyIndex >= _enemies.Count)
            return;

        _enemies[currentEnemyIndex]?.SetActive(false);

        //_enemies[currentEnemyIndex].GetComponent<Enemy>().SetCurrentHealth(_enemies[currentEnemyIndex].GetComponent<Enemy>().GetMaxHealth());
        _enemies[currentEnemyIndex].gameObject.transform.position = _spawnPoint.position;
        _enemies[currentEnemyIndex].SetActive(true);
        currentEnemyIndex++;
    }

    public bool IsWaveOver()
    {
        if (!_waitForEnemiesToDie)
            return !(currentEnemyIndex < _enemies.Count);
        else if (currentEnemyIndex > 0)
            return AreEnemiesDead();

        return false;
    }

    public bool AreEnemiesDead()
    {
        foreach (var enemy in _enemies)
        {
            if (enemy.activeSelf)
                return false;
        }
        return true;
    }
}
