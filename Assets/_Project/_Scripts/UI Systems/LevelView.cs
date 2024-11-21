using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class LevelView : MonoBehaviour {
    [Header( "Settings" )]
    [SerializeField] UIPositionAnimator animPosStageScore;
    [SerializeField] UIScaleAnimator animScaleStageScore;
    [SerializeField] TextMeshPro txtStageScore;
    [SerializeField] TextMeshPro textMultiplier;
    [SerializeField] List<UIScaleAnimator> starsView;

    [SerializeField] UIPositionAnimator animButtonPause;
    [SerializeField] UIPositionAnimator animButtonShot;
    [SerializeField] UIPositionAnimator animLevelTime;

    void OnEnable() {
        GameEvents.OnGameStart += ShowLevelUIGroup;
        GameEvents.OnGameComplete += HideLevelUIGroup;

        StageEvents.OnLevelUpData += UpdateLevelView;
        StageEvents.OnUpdateScoreData += UpdateStageScore;
        StageEvents.OnCompleteData += SendStarsToPlayerLevel;
    }

    public void TutorialComplete() => GameEvents.TutorialComplete();
    public void ButtonShotPressed() => UIEvents.ButtonShotPressed();
    public void ButtonPausePressed() => GameEvents.GamePause();
    public void SendStar() => StageEvents.SendStar();
    public void SendStarComplete() => StageEvents.SendStarComplete();

    void ShowLevelUIGroup() {
        foreach ( var star in starsView )
            star.GetComponent<UIPositionAnimator>().Hide();
        txtStageScore.text = "0";
        textMultiplier.text = "x1";

        animPosStageScore.Show();
        animButtonPause.Show();
        animButtonShot.Show();
        animLevelTime.Show();
    }

    void HideLevelUIGroup() {
        animPosStageScore.Hide();
        animButtonPause.Hide();
        animButtonShot.Hide();
        animLevelTime.Hide();
    }

    void UpdateStageScore( int newScore ) {
        txtStageScore.text = newScore.ToString( "F0" );
        animScaleStageScore.PlayAnimation();
    }

    void UpdateLevelView( int stageLevel ) {
        if ( stageLevel > 1 ) {
            starsView[ stageLevel - 2 ].PlayAnimation();
            textMultiplier.text = "x" + stageLevel;
        }
    }

    void SendStarsToPlayerLevel( int stageLevel ) {
        if ( stageLevel > 1 )
            starsView[ stageLevel - 2 ].GetComponent<UIPositionAnimator>().Show();
    }

    void OnDisable() {
        GameEvents.OnGameStart -= ShowLevelUIGroup;
        GameEvents.OnGameComplete -= HideLevelUIGroup;

        StageEvents.OnLevelUpData -= UpdateLevelView;
        StageEvents.OnUpdateScoreData -= UpdateStageScore;
        StageEvents.OnCompleteData -= SendStarsToPlayerLevel;
    }
}