using System.Collections.Generic;
using UnityEngine;
public class Mine : MonoBehaviour, IDamageable {
    [Header( "Settings" )]
    [SerializeField] MineStatsSO mineStats;
    [SerializeField] List<GameObject> oreVisuals;
    [SerializeField] MoveToTarget moveComponent;
    [SerializeField] UIScaleAnimator scaleAnimator;

    CircleCollider2D _circleCollider;
    readonly HitPointDataPool _hitPointDataPool = new HitPointDataPool();

    byte _hp;
    byte _delta;
    byte _multi;
    byte _oreCount;

    void Awake() {
        _circleCollider = GetComponent<CircleCollider2D>();
        _circleCollider.enabled = false;
    }

    public void Setup( MineData settings ) {
        _oreCount = (byte)oreVisuals.Count;
        _multi = settings.ID;
        _delta = settings.delta;
        _hp = settings.HP;
    }

    public void Activate() {
        _circleCollider.enabled = true;
        moveComponent.StartMoving();
        foreach ( var ore in oreVisuals )
            ore.SetActive( true );
    }

    public void TakeShot() {
        _hp--;
        if ( _hp <= 0 ) {
            HandleMineDestruction();
            return;
        }
        scaleAnimator.PlayAnimation();
        if ( _hp % _delta == 0 && _oreCount > 0 ) {
            // было _oreCount--; и [_oreCount]
            oreVisuals[ --_oreCount ].SetActive( false );
            _hitPointDataPool.SendHitPointData( moveComponent.Position,
                mineStats.OrePoints * _multi, EntityEvents.MineGetOre );
        } else {
            _hitPointDataPool.SendHitPointData( moveComponent.Position,
                mineStats.HitPoints * _multi, EntityEvents.MineTakeDamage );
        }
    }

    void HandleMineDestruction() {
        _hitPointDataPool.SendHitPointData( moveComponent.Position,
            mineStats.CompletedPoints * _multi, EntityEvents.MineDestroy );
        Deactivate();
    }

    public void Deactivate() {
        _circleCollider.enabled = false;
        moveComponent.StopMoving();
    }
}