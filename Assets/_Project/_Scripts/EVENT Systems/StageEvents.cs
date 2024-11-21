using System;
public class StageEvents {
    public static event Action<int> OnLevelUpData;
    public static void LevelUp( int level ) {
        OnLevelUpData?.Invoke( level );
    }

    public static event Action<int> OnCompleteData;
    public static void Complete( int level ) {
        OnCompleteData?.Invoke( level );
    }

    public static event Action<int> OnUpdateScoreData;
    public static void UpdateScore( int score ) {
        OnUpdateScoreData?.Invoke( score );
    }

    public static event Action<int> OnSendScoreData;
    public static void SendScore( int score ) {
        OnSendScoreData?.Invoke( score );
    }

    public static event Action OnSendStar;
    public static void SendStar() {
        OnSendStar?.Invoke();
    }
    public static event Action OnSendStarComplete;
    public static void SendStarComplete() {
        OnSendStarComplete?.Invoke();
    }

    public static event Action<int> OnSendMaxScoreData;
    public static void SendMaxScore( int score ) {
        OnSendMaxScoreData?.Invoke( score );
    }
}