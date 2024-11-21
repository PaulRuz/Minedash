using System;
using UnityEngine;
public class Enemy : MonoBehaviour, IPoolable, IDamageable {
    [SerializeField] UIScaleAnimator scaleAnimator;

    float _speed;
    float _speedModifier = 1f;
    int _points;
    int _multiplier;
    Vector3 _targetPosition;
    readonly HitPointDataPool _hitPointDataPool = new HitPointDataPool();

    public event Action OnDeactivated;
    public Vector3 Position { get; private set; }
    public bool IsActive { get; private set; }

    public void SpawnConfigure( EnemyTypeSO type, int multi ) {
        transform.localScale = Vector3.one * type.Scale;
        _speed = type.Speed * _speedModifier;
        _multiplier = multi;    
    }

    public void UpdateConfigure( int points, float speedMod = 1f ) {
        _points = points;
        _speedModifier = speedMod;
    }

    public void Activate( Vector3 spawnPosition, Vector3 targetPosition ) {
        _targetPosition = targetPosition;
        Position = spawnPosition;
        transform.position = Position;

        gameObject.SetActive( true );
        IsActive = true;
    }

    public void GetTopDamage() {
        _hitPointDataPool.SendHitPointData( Position, _points * _multiplier, EntityEvents.EnemyTakeDamage );
        OnDeactivated?.Invoke();
    }
    public void TakeShot() => scaleAnimator.PlayAnimation();

    void Update() {
        if ( !IsActive )
            return;
        MoveTowardsTarget();
        if ( IsAtTargetPosition() )
            OnDeactivated?.Invoke();
    }

    void MoveTowardsTarget() {
        var step = _speed * Time.deltaTime;
        Position = Vector3.MoveTowards( transform.position, _targetPosition, step );
        transform.position = Position;
    }

    bool IsAtTargetPosition() => Vector3.Distance( Position, _targetPosition ) < 0.01f;
    private void OnDisable() => IsActive = false;

}