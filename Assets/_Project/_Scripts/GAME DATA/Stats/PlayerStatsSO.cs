using UnityEngine;
using YG;

[CreateAssetMenu( fileName = "PlayerStats", menuName = "Stats/PlayerStats" )]
public class PlayerStatsSO : ScriptableObject {
    int _level;
    int _maxScore;
    public int Level { get { return _level; } }
    public int MaxScore { get { return _maxScore; } }
    public void SaveData( int level, int maxScore ) {
        _level = level;
        _maxScore = maxScore;
        YandexGame.savesData.PlayerLevel = _level;
        YandexGame.savesData.MaxScore = _maxScore;
        YandexGame.SaveProgress();
    }
}