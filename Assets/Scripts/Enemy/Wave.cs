using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Wave : MonoBehaviour
{
    public List<GameObject> _enemies = new List<GameObject>();
    public float cooldownBetweenEnemies;
    public int currentEnemyIndex;
    public float timeToNextWave;

    [SerializeField] private NaturalDisaster _disaster;
    [SerializeField] private Transform _spawnPoint;

    public NaturalDisaster Disaster => _disaster;

    private void Awake()
    {
        _disaster?.Init();
    }

    public void InitWave()
    {
        EventTriggerer.Trigger<IWaveCreateEvent>(new WaveCreateEvent(_enemies, _disaster, gameObject));

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
        _enemies[currentEnemyIndex].SetActive(false);

        //_enemies[currentEnemyIndex].GetComponent<Enemy>().SetCurrentHealth(_enemies[currentEnemyIndex].GetComponent<Enemy>().GetMaxHealth());
        _enemies[currentEnemyIndex].gameObject.transform.position = _spawnPoint.position;
        _enemies[currentEnemyIndex].SetActive(true);
        currentEnemyIndex++;
    }

    public bool IsWaveOver()
    {
        return !(currentEnemyIndex < _enemies.Count);
    }
}
