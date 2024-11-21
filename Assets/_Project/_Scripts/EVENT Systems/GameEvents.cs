using System;
public class GameEvents {
    #region Game State
    public static event Action OnGameInitComplete;
    public static void GameInitComplete() {
        OnGameInitComplete?.Invoke();
    }
    public static event Action OnGameStart;
    public static void GameStart() {
        OnGameStart?.Invoke();
    }
    public static event Action OnGameMenuEnter;
    public static void GameMenuEnter() {
        OnGameMenuEnter?.Invoke();
    }
    public static event Action OnGameMenuExit;
    public static void GameMenuExit() {
        OnGameMenuExit?.Invoke();
    }
    public static event Action OnGamePause;
    public static void GamePause() {
        OnGamePause?.Invoke();
    }
    public static event Action OnGameComplete;
    public static void GameComplete() {
        OnGameComplete?.Invoke();
    }
    public static event Action OnTutorialComplete;
    public static void TutorialComplete() {
        OnTutorialComplete?.Invoke();
    }
    #endregion
}
