using UnityEngine;

[CreateAssetMenu( fileName = "EnemyStats", menuName = "Stats/EnemyStats" )]
public class EnemyStatsSO : ScriptableObject {
    int _enemyPoints;
    float _enemySpeedMod;
    float _enemySpawnMod;
    public int EnemyPoints { get { return _enemyPoints; } }
    public float EnemySpeedMod { get { return _enemySpeedMod; } }
    public float EnemySpawnMod { get { return _enemySpawnMod; } }
    public void UpdateData( int hitPoints, float speedMod, float spawnMod ) {
        _enemyPoints = hitPoints;
        _enemySpeedMod = speedMod;
        _enemySpawnMod = spawnMod;
    }
}
