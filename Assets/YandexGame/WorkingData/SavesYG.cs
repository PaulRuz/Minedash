﻿
namespace YG {
    [System.Serializable]
    public class SavesYG {
        public int idSave;
        public bool isFirstSession = true;
        public string language = "ru";
        public bool promptDone;

        // Ваши сохранения
        public int PlayerLevel = 1;
        public int LevelStars;
        public int MaxScore;
    }
}
