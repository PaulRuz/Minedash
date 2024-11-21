using TMPro;
using UnityEngine;

public class MenuView : MonoBehaviour {
    [SerializeField] UIPositionAnimator resultScoreView;
    [SerializeField] TextMeshPro txtResultScore;
    [SerializeField] GameObject crownMaxScore;
    [SerializeField] UIPositionAnimator menuBgUp;
    [SerializeField] UIPositionAnimator menuBgDown;
    [SerializeField] UIPositionAnimator buttonStartGame;
    [SerializeField] UIPositionAnimator oreProgressView;
    [SerializeField] UIPositionAnimator maxScoreView;
    [SerializeField] TextMeshPro txtMaxScore;

    void OnEnable() {
        GameEvents.OnGameMenuExit += HideMenuUIGroup;
        GameEvents.OnGameComplete += ShowMenuUIGroup;
        PlayerLevelEvents.OnSendAllStarsToLevelComplete += ShowButtonStart;
        StageEvents.OnCompleteData += ShowButtonStart;
        StageEvents.OnSendScoreData += ShowStageScore;
        StageEvents.OnSendMaxScoreData += ShowMaxScore;
    }

    public void ButtonStartGamePressed() => GameEvents.GameMenuExit();

    void ShowButtonStart( int level ) {
        if ( level == 1 )
            buttonStartGame.Show();
    }
    void ShowButtonStart() => buttonStartGame.Show();

    void ShowMenuUIGroup() {
        menuBgUp.Show();
        menuBgDown.Show();
        resultScoreView.Show();
        oreProgressView.Show();
        maxScoreView.Show();
    }

    void HideMenuUIGroup() {
        resultScoreView.Hide();
        buttonStartGame.Hide();
        oreProgressView.Hide();
        maxScoreView.Hide();
        menuBgUp.Hide();
        menuBgDown.Hide();

        crownMaxScore.SetActive( false );
    }

    void ShowStageScore( int stageScore ) {
        txtResultScore.text = stageScore.ToString( "F0" );
    }

    void ShowMaxScore( int maxScore ) {
        crownMaxScore.SetActive( true );
        txtMaxScore.text = maxScore.ToString( "F0" );
    }

    void OnDisable() {
        GameEvents.OnGameMenuExit -= HideMenuUIGroup;
        GameEvents.OnGameComplete -= ShowMenuUIGroup;
        PlayerLevelEvents.OnSendAllStarsToLevelComplete -= ShowButtonStart;
        StageEvents.OnCompleteData -= ShowButtonStart;
        StageEvents.OnSendScoreData -= ShowStageScore;
        StageEvents.OnSendMaxScoreData -= ShowMaxScore;
    }
}