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
            _userScore = new(_scoreSteak);
        }

        public void AddDoubleScore()
        {
            _scoreSteak.Value++;
            _userScore.Value += (_scoreSteak.Value * 2);
        }

        public void AddNormalScore()
        {
            _userScore.Value++;
            _scoreSteak.Value = 0;
        }
    }
}