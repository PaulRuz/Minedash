using System;
using System.Collections;
using UnityEngine;

public abstract class BaseParticle : MonoBehaviour, IPoolable {
    protected Rigidbody2D _rb;
    protected Transform _transform;
    protected float _speed;

    public event Action OnDeactivated;

    public Transform Transform { get => _transform; }

    protected virtual void Awake() {
        _rb = GetComponent<Rigidbody2D>();
        _transform = GetComponent<Transform>();
    }


    public void Activate( Vector2 force ) {
        ConfigureParticle();        // Шаблонный метод для установки свойств
        ApplyForce( force );
        StartCoroutine( PostActivateActions() );
    }

    protected abstract void ConfigureParticle();   // Каждый тип настраивает себя
    protected virtual IEnumerator PostActivateActions() { yield break; }

    protected void ApplyForce( Vector2 force ) {
        _rb.AddForce( force * _speed, ForceMode2D.Impulse );
    }

    public virtual void Deactivate() {
        OnDeactivated?.Invoke();
        _rb.linearVelocity = Vector2.zero;
    }
}