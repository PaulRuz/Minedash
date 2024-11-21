using System.Collections;
using UnityEngine;
public class UIScaleAnimator : UIAnimator {
    [SerializeField] Vector3 targetScale;
    [SerializeField] bool pingPong = false;
    [SerializeField] bool autoReverse = false;
    Vector3 _initialScale;
    Coroutine _scaleCoroutine;

    void Awake() => _initialScale = transform.localScale;

    public override void PlayAnimation() {
        if ( _scaleCoroutine != null )
            StopAnimation();
        if ( pingPong ) {
            _scaleCoroutine = StartCoroutine( PingPongScaleCoroutine() );
        } else {
            _scaleCoroutine = StartCoroutine( Animate( _initialScale, targetScale,
                value => transform.localScale = value, autoReverse ? null : OnAnimationComplete
            ) );
            if ( autoReverse ) {
                _scaleCoroutine = StartCoroutine( Animate( targetScale, _initialScale,
                    value => transform.localScale = value, OnAnimationReset ) );
            }
        }
    }

    public void PlayReverseAnimation() {
        _scaleCoroutine = StartCoroutine( Animate( targetScale, _initialScale,
            value => transform.localScale = value, OnAnimationReset ) );
    }

    public override void StopAnimation() {
        if ( _scaleCoroutine != null ) {
            StopCoroutine( _scaleCoroutine );
            _scaleCoroutine = null;
        }
        _isAnimating = false;
    }

    IEnumerator PingPongScaleCoroutine() {
        while ( true ) {
            yield return Animate( _initialScale, targetScale, value => transform.localScale = value );
            yield return Animate( targetScale, _initialScale, value => transform.localScale = value );
        }
    }

    void OnDisable() {
        StopAllCoroutines();
        _scaleCoroutine = null;
    }
}