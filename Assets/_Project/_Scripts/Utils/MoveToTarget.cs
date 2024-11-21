using System.Collections;
using UnityEngine;

public class MoveToTarget : MonoBehaviour {
    [SerializeField] float duration = 2.0f;
    [SerializeField] AnimationCurve animationCurve = AnimationCurve.EaseInOut( 0, 0, 1, 1 );
    [SerializeField] Vector3 targetPos;
    Vector3 startPos;
    Coroutine moveCoroutine;

    public Vector3 Position { get; private set; }

    void Awake() {
        startPos = transform.position;
    }

    public void StartMoving() {
        if ( moveCoroutine != null ) {
            StopCoroutine( moveCoroutine );
        }
        moveCoroutine = StartCoroutine( MoveToTargetCoroutine() );
    }

    public void StopMoving() {
        if ( moveCoroutine != null ) {
            StopCoroutine( moveCoroutine );
            moveCoroutine = null;
        }
        Position = startPos;
        transform.position = Position;
    }

    IEnumerator MoveToTargetCoroutine() {
        float elapsedTime = 0f;
        while ( elapsedTime < duration ) {
            elapsedTime += Time.deltaTime;
            float t = animationCurve.Evaluate( elapsedTime / duration );
            Position = Vector2.Lerp( startPos, targetPos, t );
            transform.position = Position;
            yield return null;
        }
        Position = targetPos;
        transform.position = Position;
    }
}