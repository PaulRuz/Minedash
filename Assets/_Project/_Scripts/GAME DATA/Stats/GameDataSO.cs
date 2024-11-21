using UnityEngine;

[CreateAssetMenu( fileName = "GameData", menuName = "Stats/GameData" )]
public class GameDataSO : ScriptableObject {
    public MineStatsSO MineStats;
    public EnemyStatsSO EnemyStats;
    public PlayerStatsSO PlayerStats;
    public void UpdateEntityStats( PlayerLevelSettings settings ) {
        MineStats.UpdateData( settings.HitPoints, settings.OrePoints, settings.CompletePoints );
        EnemyStats.UpdateData( settings.EnemyPoints, settings.EnemySpeedMod, settings.EnemySpawnMod );
    }
    public void UpdatePlayerStats( int level, int maxScore ) {
        PlayerStats.SaveData( level, maxScore );
    }
}
