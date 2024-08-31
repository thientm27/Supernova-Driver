using Elympics;
using UnityEngine;

namespace SupernovaDriver.Scripts.SceneController.Game
{
    public class GameScoreHandler : ElympicsMonoBehaviour
    {
        [SerializeField] private int maxScoreSteak;

        private ElympicsInt _userScore;
        private ElympicsInt _scoreSteak;

        public void Initialize()
        {
            _userScore = new(0);
            _scoreSteak = new(_scoreSteak);
        }

        public int AddDoubleScore()
        {
            _scoreSteak.Value++;
            var scoreGet = (_scoreSteak.Value * 2);
            _userScore.Value += scoreGet;
            return scoreGet;
        }

        public void AddNormalScore()
        {
            _userScore.Value++;
            _scoreSteak.Value = 0;
        }
    }
}