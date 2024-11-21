using UnityEngine;

public class LevelTime : MonoBehaviour {
    [Header( "References" )]
    [SerializeField] Transform progressTransform;
    [SerializeField] UIPositionAnimator progressDamageAnimator;
    [SerializeField] UIScaleAnimator clockView;
    [Header( "Settings" )]
    [SerializeField] float maxSpeed = 0.024f;
    [SerializeField] float maxPositionX = 46f;
    [SerializeField] float speedModifierFactor = 0.3f;
    [SerializeField] float minSpeedModifier = 0.7f;
    [SerializeField] float penalty = 0.05f;

    bool _isRunning;
    float _progressValue = 1f;
    float _speed;
    float _deltaValue;

    void OnEnable() {
        GameEvents.OnTutorialComplete += StartProgress;
        GameEvents.OnGameMenuExit += ResetProgress;

        EntityEvents.OnBulletTakeDamage += ApplyPenalty;
    }

    void StartProgress() {
        _isRunning = true;
    }

    void ResetProgress() {
        _progressValue = 1f;
        _speed = 0f;
        _deltaValue = 0f;
        UpdateTransformPosition();
    }

    void ApplyPenalty() {
        if ( _isRunning == false )
            return;

        _progressValue = Mathf.Max( 0, _progressValue - penalty );  // Обеспечивает, что прогресс не станет отрицательным

        if ( _progressValue <= 0 ) {
            _isRunning = false;
            GameEvents.GameComplete();
        }

        UpdateTransformPosition();
        progressDamageAnimator.Show();
    }

    void Update() {
        if ( _isRunning == false)
            return;

        if ( _progressValue <= 0f ) {
            _isRunning = false;
            GameEvents.GameComplete();
            return;
        }

        UpdateProgressValue();
        UpdateTransformPosition();
    }

    void UpdateProgressValue() {
        _speed = CalculateCurrentSpeed();
        _deltaValue = _speed * Time.deltaTime;
        _progressValue -= _deltaValue;
    }

    private float CalculateCurrentSpeed() {
        float speedModifier = _progressValue * speedModifierFactor + minSpeedModifier;
        return maxSpeed * speedModifier;
    }

    void UpdateTransformPosition() {
        Vector3 newScale = progressTransform.localScale;
        newScale.x = _progressValue * maxPositionX;
        progressTransform.localScale = newScale;
    }

    void OnDisable() {
        GameEvents.OnTutorialComplete -= StartProgress;
        GameEvents.OnGameMenuExit -= ResetProgress;

        EntityEvents.OnBulletTakeDamage -= ApplyPenalty;
    }
}