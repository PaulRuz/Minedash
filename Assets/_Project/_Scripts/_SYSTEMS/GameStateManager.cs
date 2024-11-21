using System.Collections;
using UnityEngine;
using YG;
public class GameStateManager : MonoBehaviour, IInitializable {
    GameState _currentState;

    public IEnumerator Initialize() {
        _currentState = GameState.Init;
        yield return null;
    }

    void OnEnable() {
        GameEvents.OnGameInitComplete += GameStart;
        GameEvents.OnGameMenuExit += GameStart;
        GameEvents.OnGamePause += GamePause;
        GameEvents.OnGameComplete += GameComplete;
    }

    void GameStart() {
        YandexGame.FullscreenShow();
        YandexGame.GameplayStart();
        GameEvents.GameStart();

        _currentState = GameState.Playing;
    }

    void GamePause() {
        if ( _currentState == GameState.Playing ) {
            _currentState = GameState.Paused;
            Time.timeScale = 0;
            YandexGame.GameplayStop();
        } else if ( _currentState == GameState.Paused ) {
            _currentState = GameState.Playing;
            Time.timeScale = 1;
            YandexGame.GameplayStart();
        }
    }

    void GameComplete() {
        YandexGame.GameplayStop();
        _currentState = GameState.Stopped;
    }

    enum GameState {
        Menu, Starting, Playing,
        Paused, Stopped, Init
    }

    void OnDisable() {
        GameEvents.OnGameInitComplete -= GameStart;
        GameEvents.OnGamePause -= GamePause;
        GameEvents.OnGameComplete -= GameComplete;
    }
}

