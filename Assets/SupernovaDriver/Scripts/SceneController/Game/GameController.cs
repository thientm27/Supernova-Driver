using System;
using Imba.UI;
using SupernovaDriver.Scripts.UI.View;
using UnityEngine;

namespace SupernovaDriver.Scripts.SceneController.Game
{
    public class GameController : MonoBehaviour
    {
        private int _userScore = 0;

        private GameView _gameView;

        private void Start()
        {
            _gameView = UIManager.Instance.ViewManager.GetViewByName<GameView>(UIViewName.GameView);
            _gameView.Show();
            _gameView.SetDisplayScore(0);
        }

        public void OnGetScore(int scoreGot)
        {
            _userScore += 1;
        }
    }
}