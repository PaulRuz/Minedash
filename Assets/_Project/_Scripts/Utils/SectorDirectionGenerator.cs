using UnityEngine;

public class SectorDirectionGenerator {
    float sectorAngle;
    float sectorRotation;
    public SectorDirectionGenerator( float sectorAngle, float sectorRotation = 0f ) {
        this.sectorAngle = sectorAngle;
        this.sectorRotation = sectorRotation;
    }
    public Vector2 AngleToVectorInSector() {
        var angle = Random.Range(0, sectorAngle);
        var angleMiddleDelta = ( 180 - sectorRotation - sectorAngle ) / 2;
        return GetUnitOnCircle( angle + angleMiddleDelta );
    }
    Vector3 GetUnitOnCircle( float angleDegress ) {
        var angleRadians = angleDegress * Mathf.PI / 180.0f;
        var x = Mathf.Cos( angleRadians );
        var y = Mathf.Sin( angleRadians );
        return new Vector3( x, y, 0 );
    }
}