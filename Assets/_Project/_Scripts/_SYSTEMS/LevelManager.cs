using System.Collections;
using UnityEngine;
using YG;
public class LevelManager : MonoBehaviour, IInitializable {
    int _stageScore;
    int _stageLevel;
    int _maxStageScore;

    const byte InitialLevel = 1;
    const byte MaxStageLevel = 10;

    public IEnumerator Initialize() {
        yield return null;
    }

    void OnEnable() {
        GameEvents.OnGameComplete += SendScore;
        GameEvents.OnGameMenuEnter += SendStarToPlayerLevel;
        GameEvents.OnGameStart += StartStage;

        EntityEvents.OnMineDestroy += LevelUp;
        EntityEvents.OnMineDestroyData += GetPoints;
        EntityEvents.OnMineGetOreData += GetPoints;
        EntityEvents.OnMineTakeDamageData += GetPoints;
        EntityEvents.OnEnemyTakeDamageData += GetPoints;
    }

    void StartStage() {
        _stageScore = 0;
        _stageLevel = InitialLevel;
        StageEvents.LevelUp( _stageLevel );
    }

    void GetPoints( HitPointData hitData ) {
        _stageScore += hitData.Points;
        StageEvents.UpdateScore( _stageScore );
    }

    void SendScore() {
        StageEvents.SendScore( _stageScore );

        var prevMaxScore = YandexGame.savesData.MaxScore;
        if ( _stageScore > prevMaxScore ) {
            YandexGame.savesData.MaxScore = _stageScore;
            YandexGame.NewLeaderboardScores( "leaderMD", _stageScore );
            YandexGame.SaveProgress();
            StageEvents.SendMaxScore( _stageScore );
        } else {
            StageEvents.SendMaxScore( prevMaxScore );
        }
    }

    void SendStarToPlayerLevel() => StageEvents.Complete( _stageLevel );

    void LevelUp() {
        _stageLevel++;
        if ( _stageLevel > MaxStageLevel )
            _stageLevel = MaxStageLevel;
        StageEvents.LevelUp( _stageLevel );
    }

    void OnDisable() {
        GameEvents.OnGameComplete -= SendScore;
        GameEvents.OnGameMenuEnter -= SendStarToPlayerLevel;
        GameEvents.OnGameStart -= StartStage;

        EntityEvents.OnMineDestroy -= LevelUp;
        EntityEvents.OnMineDestroyData -= GetPoints;
        EntityEvents.OnMineGetOreData -= GetPoints;
        EntityEvents.OnMineTakeDamageData -= GetPoints;
        EntityEvents.OnEnemyTakeDamageData -= GetPoints;
    }
}