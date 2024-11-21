using UnityEngine;
using UnityEngine.Events;

public class StartTutorial : MonoBehaviour {
    [SerializeField] UIScaleAnimator pointer;
    [SerializeField] CircleCollider2D pointerCollider;
    [SerializeField] UnityEvent OnExitTutorial;
    bool _isTutorialComplete = false;

    public void Launch() {
        if ( _isTutorialComplete )
            _isTutorialComplete = false;
        pointer.gameObject.SetActive( true );
        pointerCollider.enabled = true;
        pointer.PlayAnimation();
    }

    public void Exit() {
        if ( _isTutorialComplete )
            return;
        pointerCollider.enabled = false;
        OnExitTutorial?.Invoke();
        pointer.StopAnimation();
        pointer.gameObject.SetActive( false );
        _isTutorialComplete = true;
    }
}
