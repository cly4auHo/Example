using System;

namespace Network
{
    [Serializable]
    public class GameModel
    {
        public float GameSessionTime;
        public int BoosterAdditional;
        public int AmountOfAnswers;
        public int ChangeDifficulty;

        public int EasyMinValue;
        public int EasyMaxValue;
        public int ScoreEasy;
        public float AdditionalTimeEasy;

        public int MediumMinValue;
        public int MediumMaxValue;
        public int ScoreMedium;
        public float AdditionalTimeMedium;

        public int HardMinValue;
        public int HardMaxValue;
        public int ScoreHard;
        public float AdditionalTimeHard;

        public static GameModel DEFAULT => new GameModel
        {
            GameSessionTime = 60,
            BoosterAdditional = 5,
            AmountOfAnswers = 4,
            ChangeDifficulty = 10,
            EasyMinValue = 0,
            EasyMaxValue = 10,
            ScoreEasy = 10,
            AdditionalTimeEasy = 1,
            MediumMinValue = 0,
            MediumMaxValue = 100,
            ScoreMedium = 20,
            AdditionalTimeMedium = 2,
            HardMinValue = 0,
            HardMaxValue = 1000,
            ScoreHard = 30,
            AdditionalTimeHard = 3
        };
    }
}