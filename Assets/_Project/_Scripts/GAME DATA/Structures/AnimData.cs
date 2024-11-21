using UnityEngine;
public struct AnimData {
    public Vector3 Position;
    public float Duration;
    public AnimData( Vector3 position, float duration) {
        Position = position;
        Duration = duration;
    }
}