using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
public abstract class UIAnimator : MonoBehaviour {
    [Header( "Settings" )]
    [SerializeField] protected float duration = 0.5f;
    [SerializeField] protected AnimationCurve animationCurve = AnimationCurve.EaseInOut( 0, 0, 1, 1 );
    [Header( "Events" )]
    [SerializeField] protected UnityEvent OnAnimationStart;
    [SerializeField] protected UnityEvent OnAnimationComplete;
    [SerializeField] protected UnityEvent OnAnimationReset;
    protected bool _isAnimating;

    public bool IsAnimating { get { return _isAnimating; } }

    public abstract void PlayAnimation();
    public abstract void StopAnimation();

    protected IEnumerator Animate( Vector3 startValue, Vector3 targetValue, Action<Vector3> updateValue, UnityEvent eventComplete = null ) {
        _isAnimating = true;
        OnAnimationStart?.Invoke();

        float elapsedTime = 0f;
        while ( elapsedTime < duration ) {
            // Прекращаем корутину, если объект уничтожен:
            if ( this == null )
                yield break;

            elapsedTime += Time.deltaTime;
            float t = animationCurve.Evaluate( elapsedTime / duration );
            updateValue( Vector3.Lerp( startValue, targetValue, t ) );
            yield return null;
        }

        if ( this != null )
            updateValue( targetValue );
        _isAnimating = false;
        eventComplete?.Invoke();
    }
}