using UnityEngine;

[RequireComponent( typeof( Rigidbody2D ) )]
public class ObjectParticle : BaseParticle {
    [SerializeField] float baseSpeed = 1f;
    [SerializeField] float randomSpeedFactor = 0.2f;

    protected override void ConfigureParticle() {
        _speed = Random.Range( baseSpeed * ( 1 - randomSpeedFactor ), 
            baseSpeed * ( 1 + randomSpeedFactor ) );
    }
}
