using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour
{
    public HashSet<GameObject> _enemyList = new HashSet<GameObject>();
    
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private float _minSpawnTime;
    [SerializeField] private float _maxSpawnTime;
    [SerializeField] private int _maxEnemiesOnSpawner = 10;

    private float _timeUntilSpawn;

    private void SetTimeUntilSpawn()
    {
        _timeUntilSpawn = Random.Range(_minSpawnTime, _maxSpawnTime);
    }

    private void Awake()
    {
        SetTimeUntilSpawn();
    }

    private void Update()
    {
        _timeUntilSpawn -= Time.deltaTime;

        if (_timeUntilSpawn <= 0 && _enemyList.Count < _maxEnemiesOnSpawner)
        {
            GameObject obj = Instantiate(_enemyPrefab, transform.position, Quaternion.identity);
            SetTimeUntilSpawn();
            _enemyList.Add(obj);

            var _npcEvents = obj.GetComponent<NPCHealthEvents>();

            if (_npcEvents._spawnerParent == null)
            {
                _npcEvents._spawnerParent = gameObject;
            }
        }
    }
}
