using System.Collections;
using UnityEngine;
using YG;
public class PlayerLevelManager : MonoBehaviour, IInitializable {
    [Header( "Settings" )]
    [SerializeField] PlayerLevelConfig playerLevelConfig;
    [SerializeField] GameDataSO gameData;
    [SerializeField] Material oreMaterial;
    [SerializeField] Material envMaterial;

    #region private Var
    int _playerLevel;
    int _maxLevelStars;
    int _levelStars;
    int _excessStars;

    static readonly int MaxPlayerLevelID = 9;
    #endregion

    public IEnumerator Initialize() {
        LoadProgress();
        PlayerLevelEvents.ProgressComplete( (float)_levelStars / _maxLevelStars );
        yield return null;
    }

    void OnEnable() {
        GameEvents.OnGameInitComplete += UpdatePlayerInfo;

        StageEvents.OnSendStar += AddStar;
        StageEvents.OnSendStarComplete += CompleteStarsProgress;
    }

    void UpdatePlayerInfo() => PlayerLevelEvents.UpdateLevel( _playerLevel );

    void AddStar() {
        PlayerLevelEvents.SendStarToLevel();
        _levelStars++;

        if ( _levelStars >= _maxLevelStars )
            _excessStars++;
        else
            PlayerLevelEvents.ProgressComplete( (float)_levelStars / _maxLevelStars );
    }

    void CompleteStarsProgress() {
        if ( _levelStars >= _maxLevelStars ) {
            LevelUp();
        } else if ( _levelStars > 0 ) {
            YandexGame.savesData.LevelStars = _levelStars;
            YandexGame.SaveProgress();
            PlayerLevelEvents.ProgressComplete( (float)_levelStars / _maxLevelStars );
            PlayerLevelEvents.SendAllStarsToLevelComplete();
        }
    }

    #region private Methods
    void LoadProgress() {
        _playerLevel = YandexGame.savesData.PlayerLevel;
        _levelStars = YandexGame.savesData.LevelStars;
        ConfigureLevelSettings();
    }

    void LevelUp() {
        _playerLevel++;
        YandexGame.savesData.PlayerLevel = _playerLevel;
        YandexGame.savesData.LevelStars = _excessStars;
        YandexGame.SaveProgress();

        gameData.UpdatePlayerStats( _playerLevel, YandexGame.savesData.MaxScore );
        _levelStars = _excessStars;
        _excessStars = 0;

        ConfigureLevelSettings();
        PlayerLevelEvents.ProgressComplete( (float)_levelStars / _maxLevelStars );
        PlayerLevelEvents.LevelUp( _playerLevel );

        PlayerLevelEvents.SendAllStarsToLevelComplete();
        _levelStars = 0;
    }

    void ConfigureLevelSettings() {
        var settings = playerLevelConfig.GetItemByID(
            (byte)Mathf.Min( _playerLevel, MaxPlayerLevelID ) );
        _maxLevelStars = settings.MaxLevelStars;
        gameData.UpdateEntityStats( settings );
        gameData.UpdatePlayerStats( _playerLevel, YandexGame.savesData.MaxScore );
        oreMaterial.mainTexture = settings.OreTexture;
        envMaterial.mainTexture = settings.EnvTexture;
    }

    void OnApplicationQuit() {
        YandexGame.savesData.LevelStars = _levelStars;
        YandexGame.SaveProgress();
    }

    #endregion

    void OnDisable() {
        GameEvents.OnGameInitComplete -= UpdatePlayerInfo;

        StageEvents.OnSendStar -= AddStar;
        StageEvents.OnSendStarComplete -= CompleteStarsProgress;
    }
}