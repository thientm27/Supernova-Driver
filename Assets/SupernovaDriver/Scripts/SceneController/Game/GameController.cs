using Elympics;
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
        [SerializeField] private CarScript         carScript;
        [SerializeField] private GameServerHandler serverHandler;
        [SerializeField] private GameScoreHandler gameScoreHandler;


        private GameView _gameView;

        private void Start()
        {
            PauseGame();
            _gameView = UIManager.Instance.ViewManager.GetViewByName<GameView>(UIViewName.GameView);
        }

        public void StartGame(bool isClient)
        {
            _gameView.Show();
            _gameView.SetDisplayScore(0);
            if (isClient)
            {
                StartCoroutine(_gameView.StartGameCountDown(null, null));
            }
            else
            {
                StartCoroutine(_gameView.StartGameCountDown(
                    () => { carScript.Init(); }
                    , ResumeGame
                ));
            }
        }

        public void PauseGame()
        {
            carScript.Pause();
        }

        public void ResumeGame()
        {
            carScript.Resume();
        }

      
        public void ReLoadGame()
        {
            SceneManager.LoadScene(Constants.GameScene);
        }

        public void EndGame(bool isClient)
        {
            if (!isClient)
            {
                serverHandler.EndGameServer();
            }
            
            UIManager.Instance.PopupManager.ShowPopup(UIPopupName.EndGamePopup, new EndGameParam
            {
                userScore        = 0,
                newScore         = false,
                reloadGameAction = ReLoadGame
            });
        }

        public void GotScore(bool isDouble)
        {
            var scoreGot = 1;
            if (isDouble)
            {
                scoreGot = gameScoreHandler.AddDoubleScore();;
            }
            else
            {
                gameScoreHandler.AddNormalScore();;

            }
            // do some effect
        }
    }
}