using System;
using UnityEngine;
public class Bullet : MonoBehaviour, IPoolable {
    [Header( "Settings" )]
    [SerializeField] float speed = 10f;
    [SerializeField] GameObject moveFX;
    [SerializeField] UIPositionAnimator showComponent;
    [SerializeField] Rigidbody2D rb;

    private Vector3 _initPosition;
    private bool _isMove;

    public event Action OnDeactivated;
    readonly HitPointDataPool _hitPointDataPool = new HitPointDataPool();

    void Awake() => _initPosition = transform.position;

    public void Activate() {
        ResetBulletState();
        gameObject.SetActive( true );
        showComponent.Show();
    }

    public void ShowComplete() => EntityEvents.BulletShow();

    public void SetMove() {
        _isMove = !_isMove;
        ToggleMoveFX( _isMove );
    }
    public void Deactivate() {
        SetMove();
        OnDeactivated?.Invoke();
    }

    void FixedUpdate() {
        if ( _isMove )
            MoveBullet();
    }

    void OnTriggerEnter2D( Collider2D collision ) {
        if ( collision.TryGetComponent<IDamageable>( out var target ) ) {
            target.TakeShot();
            if ( collision.GetComponent<Enemy>() )
                _hitPointDataPool.SendHitPointData( transform.position, 5, EntityEvents.BulletTakeDamage );
        }
        Deactivate();
    }

    void MoveBullet() {
        Vector2 direction = Vector2.up;
        rb.MovePosition( rb.position + direction * speed * Time.fixedDeltaTime );
    }

    void ResetBulletState() {
        _isMove = false;
        transform.position = _initPosition;
        ToggleMoveFX( false );
    }

    void ToggleMoveFX( bool isActive ) {
        if ( moveFX != null )
            moveFX.SetActive( isActive );
    }

    void OnDisable() {
        ResetBulletState();
    }
}