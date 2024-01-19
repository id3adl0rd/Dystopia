using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHealthEvents : MonoBehaviour
{
    public GameObject _spawnerParent;
    
    public void OnNPCDeath()
    {
        if (_spawnerParent != null && _spawnerParent.GetComponent<EnemySpawnerController>())
        {
            var _spawner = _spawnerParent.GetComponent<EnemySpawnerController>();
            _spawner._enemyList.Remove(gameObject);
        }
    }
}
