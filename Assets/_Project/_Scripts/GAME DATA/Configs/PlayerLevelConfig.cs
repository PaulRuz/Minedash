using UnityEngine;

[CreateAssetMenu( fileName = "PlayerLevelConfig", menuName = "Configs/PlayerLevelConfig" )]
public class PlayerLevelConfig : ConfigBase<PlayerLevelSettings> {
    protected override byte GetID( PlayerLevelSettings item ) {
        return item.ID;
    }
}

[System.Serializable]
public class PlayerLevelSettings {
    public byte ID;
    public Texture2D OreTexture;
    public Texture2D EnvTexture;
    public int MaxLevelStars;

    public int HitPoints;
    public int OrePoints;
    public int CompletePoints;

    public int EnemyPoints;
    public float EnemySpeedMod;
    public float EnemySpawnMod;
}
