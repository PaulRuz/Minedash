using System;
public class PlayerLevelEvents {
    #region Player
    public static event Action OnPlayerLevelUp;
    public static event Action<int> OnPlayerLevelUpData;
    public static void LevelUp( int level ) {
        OnPlayerLevelUp?.Invoke();
        OnPlayerLevelUpData?.Invoke( level );
    }
    public static event Action<float> OnProgressCompleteData;
    public static void ProgressComplete(float progress) {
        OnProgressCompleteData?.Invoke( progress );
    }
    public static event Action OnSendStarToLevel;
    public static void SendStarToLevel() {
        OnSendStarToLevel?.Invoke();
    }
    public static event Action OnSendAllStarsToLevelComplete;
    public static void SendAllStarsToLevelComplete() {
        OnSendAllStarsToLevelComplete?.Invoke();
    }

    public static event Action<int> OnLoadData;
    public static void LoadData( int level ) {
        OnLoadData?.Invoke( level );
    }

    public static event Action<int> OnUpdateLevel;
    public static void UpdateLevel( int level ) {
        OnUpdateLevel?.Invoke( level );
    }
    #endregion
}
