using UnityEngine;

[CreateAssetMenu( fileName = "MineStats", menuName = "Stats/MineStats" )]
public class MineStatsSO : ScriptableObject {
    int _hitPoints;
    int _orePoints;
    int _completePoints;
    public int HitPoints { get { return _hitPoints; } }
    public int OrePoints { get { return _orePoints; } }
    public int CompletedPoints { get { return _completePoints; } }
    public void UpdateData( int hitPoints, int orePoints, int completePoints ) {
        _hitPoints = hitPoints;
        _orePoints = orePoints;
        _completePoints = completePoints;
    }
}
