using UnityEngine;

[CreateAssetMenu( fileName = "EnemyConfig", menuName = "Configs/EnemyConfig" )]
public class EnemyConfig : ConfigBase<EnemyData> {
    protected override byte GetID( EnemyData item ) {
        return item.ID;
    }
}

[System.Serializable]
public class EnemyData {
    public byte ID;
    public float MinSpawnTime;
    public float MaxSpawnTime;
    public byte ChanceSpawnDown;
    public byte ChanceSpawnMiddle;
    public byte ChanceSpawnUp;
}