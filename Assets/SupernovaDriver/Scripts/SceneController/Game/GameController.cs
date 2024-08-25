using System;
using Imba.UI;
using Imba.Utils;
using SupernovaDriver.Scripts.SceneController.Game.Entity;
using SupernovaDriver.Scripts.UI.Popups;
using SupernovaDriver.Scripts.UI.View;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SupernovaDriver.Scripts.SceneController.Game
{
    public class GameController : ManualSingletonMono<GameController>
    {
        [SerializeField] private CarScript carScript;

        private int _userScore = 0;

        private GameView _gameView;

        private void Start()
        {
            PauseGame();
            _gameView = UIManager.Instance.ViewManager.GetViewByName<GameView>(UIViewName.GameView);
            _gameView.Show();
            _gameView.SetDisplayScore(0);

            StartCoroutine(_gameView.StartGameCountDown(
                () => { carScript.Init(); }
                , ResumeGame
            ));
        }

        public void PauseGame()
        {
            carScript.Pause();
        }

        public void ResumeGame()
        {
            carScript.Resume();
        }

        public void OnGetScore(int scoreGot)
        {
            _userScore += 1;
        }

        public void ReLoadGame()
        {
            SceneManager.LoadScene(Constants.GameScene);
        }

        public void EndGame()
        {
            UIManager.Instance.PopupManager.ShowPopup(UIPopupName.EndGamePopup, new EndGameParam
            {
                userScore        = _userScore,
                newScore         = false,
                reloadGameAction = ReLoadGame
            });
        }
    }
}