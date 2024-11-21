using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyManager : MonoBehaviour, IInitializable {
    [Header( "Settings" )]
    [SerializeField] EnemyStatsSO enemyStats;
    [SerializeField] EnemyConfig enemyConfig;
    [SerializeField] List<EnemyTypeSO> enemyTypes;
    [SerializeField] List<EnemySpawner> enemySpawners;
    [SerializeField] Enemy enemyPrefab;

    ObjectPool<Enemy> _enemyPool;
    Coroutine _spawnCoroutine;
    bool _isSpawningEnabled;

    EnemyData _enemySettings;
    float _minSpawnTime;
    float _maxSpawnTime;

    void OnEnable() {
        GameEvents.OnTutorialComplete += LaunchEnemySpawners;
        GameEvents.OnGameMenuEnter += StopSpawning;
        StageEvents.OnLevelUpData += SetEnemiesLevel;
    }

    public IEnumerator Initialize() {
        _isSpawningEnabled = false;
        _enemyPool = new ObjectPool<Enemy>( enemyPrefab, transform );
        foreach ( var spawner in enemySpawners )
            spawner.Initialize( _enemyPool, enemyTypes );
        yield return null;
    }

    #region Spawn Control
    void LaunchEnemySpawners() {
        SetEnemiesLevel( 1 );

        _isSpawningEnabled = true;
        _spawnCoroutine = StartCoroutine( SpawnEnemyRoutine() );
    }
    void StopSpawning() {
        _isSpawningEnabled = false;
        StopCoroutine( _spawnCoroutine );
        _enemyPool.DeactivateAllObjects();
    }
    #endregion

    void SetEnemiesLevel( int newLevel ) {
        _enemySettings = enemyConfig.GetItemByID( (byte)newLevel );
        _minSpawnTime = _enemySettings.MinSpawnTime * enemyStats.EnemySpawnMod;
        _maxSpawnTime = _enemySettings.MaxSpawnTime * enemyStats.EnemySpawnMod;
        CalculateSpawnerChance();
        if ( newLevel > 1 ) {
            int newPoints = enemyStats.EnemyPoints * newLevel;
            var enemiesInQueue = _enemyPool.GetQueue();
            var enemiesInHash = _enemyPool.GetHash();
            foreach ( var enemy in enemiesInQueue )
                enemy.UpdateConfigure( newPoints, enemyStats.EnemySpeedMod );
            foreach ( var enemy in enemiesInHash )
                enemy.UpdateConfigure( newPoints, enemyStats.EnemySpeedMod );

            UpdateSpawnTime();
        }
    }

    void UpdateSpawnTime() {
        if ( _spawnCoroutine != null ) {
            StopCoroutine( _spawnCoroutine );
            _spawnCoroutine = StartCoroutine( SpawnEnemyRoutine() );
        }
    }

    IEnumerator SpawnEnemyRoutine() {
        while ( _isSpawningEnabled ) {
            yield return new WaitForSeconds( Random.Range( _minSpawnTime, _maxSpawnTime ) );
            GetSpawnerByChance()?.SpawnEnemy();
        }
    }

    int _totalChance;
    int[] _cumulativeChances;
    EnemySpawner GetSpawnerByChance() {
        int randomValue = Random.Range( 0, _totalChance );
        for ( int i = 0 ; i < _cumulativeChances.Length ; i++ ) {
            if ( !enemySpawners[ i ].IsSpawning && randomValue < _cumulativeChances[ i ] )
                return enemySpawners[ i ];
        }
        return enemySpawners[ Random.Range( 0, enemySpawners.Count - 1 ) ];
    }

    void CalculateSpawnerChance() {
        _totalChance = _enemySettings.ChanceSpawnDown
            + _enemySettings.ChanceSpawnMiddle
            + _enemySettings.ChanceSpawnUp;
        _cumulativeChances = new int[] {
            _enemySettings.ChanceSpawnDown,
            _enemySettings.ChanceSpawnDown + _enemySettings.ChanceSpawnMiddle,
            _totalChance
        };
    }

    void OnDisable() {
        GameEvents.OnTutorialComplete -= LaunchEnemySpawners;
        GameEvents.OnGameMenuEnter -= StopSpawning;
        StageEvents.OnLevelUpData -= SetEnemiesLevel;
    }
}