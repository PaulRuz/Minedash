using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    [SerializeField] int ID;
    [SerializeField] EnemyStatsSO enemyStats;
    [SerializeField] Transform spawnPointA;
    [SerializeField] Transform spawnPointB;
    ObjectPool<Enemy> _enemyPool;
    List<EnemyTypeSO> _enemyTypes;
    Enemy _spawnedEnemy;

    public bool IsSpawning => _spawnedEnemy?.IsActive ?? false;
    public bool IsSpawnDirAB;

    public void Initialize( ObjectPool<Enemy> pool, List<EnemyTypeSO> types ) {
        _enemyPool = pool;
        _enemyTypes = types;
    }

    public void SpawnEnemy() {
        var enemyType = _enemyTypes[ Random.Range( 0, _enemyTypes.Count ) ];
        var (spawnPos, targetPos) = GetSpawnAndTargetPoints();

        _spawnedEnemy = _enemyPool.GetNextObject();

        _spawnedEnemy.SpawnConfigure( enemyType, ID );
        _spawnedEnemy.Activate( spawnPos, targetPos );
    }

    (Vector3 spawnPos, Vector3 targetPos) GetSpawnAndTargetPoints() {
        if ( IsSpawning ) {
            return IsSpawnDirAB
            ? (spawnPointA.position, spawnPointB.position)
            : (spawnPointB.position, spawnPointA.position);
        }
        IsSpawnDirAB = Random.value > 0.5f ? true : false;
        return IsSpawnDirAB
            ? (spawnPointA.position, spawnPointB.position)
            : (spawnPointB.position, spawnPointA.position);
    }
}