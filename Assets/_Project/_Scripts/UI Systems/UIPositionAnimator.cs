using UnityEngine;
using UnityEngine.Events;

public class UIPositionAnimator : UIAnimator {
    [SerializeField] UnityEvent OnStartHideAnimation;
    [SerializeField] Vector3 targetPosition;

    Vector3 initialPosition;
    Coroutine animationCoroutine;

    void Awake() {
        _isAnimating = false;
        initialPosition = transform.localPosition;
    }

    public void Show() {
        PlayAnimationToPosition( initialPosition, targetPosition, OnAnimationComplete );
    }

    public void Hide() {
        OnStartHideAnimation?.Invoke();
        PlayAnimationToPosition( targetPosition, initialPosition, OnAnimationReset );
    }

    public override void PlayAnimation() {
        Show();
    }

    public override void StopAnimation() {
        if ( animationCoroutine != null ) {
            StopCoroutine( animationCoroutine );
            _isAnimating = false;
        }
    }

    void PlayAnimationToPosition( Vector3 from, Vector3 to, UnityEvent gameEvent = null ) {
        if ( _isAnimating )
            StopAnimation();

        animationCoroutine = StartCoroutine( Animate( from, to,
            value => transform.localPosition = value, gameEvent ) );
    }
}
