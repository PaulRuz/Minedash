using UnityEngine;

[CreateAssetMenu( fileName = "MineConfig", menuName = "Configs/MineConfig" )]
public class MineConfig : ConfigBase<MineData> {
    protected override byte GetID( MineData item ) {
        return item.ID;
    }
}

[System.Serializable]
public class MineData {
    public byte ID;
    public byte HP;
    public byte delta;
}
