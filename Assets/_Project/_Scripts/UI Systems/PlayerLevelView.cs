using TMPro;
using UnityEngine;

public class PlayerLevelView : MonoBehaviour {
    [SerializeField] UIPositionAnimator transformAnim;
    [SerializeField] UIScaleAnimator animStarLevelUp;
    [SerializeField] TextMeshPro txtPlayerLevel;
    [SerializeField] UIScaleAnimator animStarOutlineFX;
    [SerializeField] Transform progressBar;

    UIPositionAnimator _animView;

    static readonly float MaxWidth = 43f;

    void OnEnable() {
        GameEvents.OnGameComplete += ShowPlayerLevelView;
        GameEvents.OnGameMenuExit += HideShowPlayerLevelView;
        PlayerLevelEvents.OnSendStarToLevel += AnimateGetStar;
        PlayerLevelEvents.OnPlayerLevelUp += AnimateLevelUp;
        PlayerLevelEvents.OnPlayerLevelUpData += UpdatePlayerLevelView;
        PlayerLevelEvents.OnUpdateLevel += UpdatePlayerLevelView;
        PlayerLevelEvents.OnProgressCompleteData += UpdateProgressView;
    }

    public void ShowComplete() => GameEvents.GameMenuEnter();

    void Awake() {
        _animView = GetComponent<UIPositionAnimator>();
    }

    void ShowPlayerLevelView() {
        transformAnim.Show();
    }
    void HideShowPlayerLevelView() {
        transformAnim.Hide();
    }

    void AnimateGetStar() => animStarLevelUp.PlayAnimation();

    void AnimateLevelUp() {
        animStarLevelUp.PlayAnimation();
        animStarOutlineFX.PlayAnimation();
    }

    void UpdatePlayerLevelView( int level ) =>
       txtPlayerLevel.text = level.ToString();

    void UpdateProgressView( float value ) =>
       progressBar.localScale = new Vector3( value * MaxWidth, 1f, 1f );

    void OnDisable() {
        GameEvents.OnGameComplete -= ShowPlayerLevelView;
        GameEvents.OnGameMenuExit -= HideShowPlayerLevelView;
        PlayerLevelEvents.OnSendStarToLevel -= AnimateGetStar;
        PlayerLevelEvents.OnPlayerLevelUp -= AnimateLevelUp;
        PlayerLevelEvents.OnPlayerLevelUpData -= UpdatePlayerLevelView;
        PlayerLevelEvents.OnUpdateLevel -= UpdatePlayerLevelView;
        PlayerLevelEvents.OnProgressCompleteData -= UpdateProgressView;
    }
}