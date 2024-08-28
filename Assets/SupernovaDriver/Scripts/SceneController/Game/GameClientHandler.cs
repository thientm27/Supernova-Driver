using System;
using Elympics;

namespace SupernovaDriver.Scripts.SceneController.Game
{
    public class GameClientHandler : ElympicsMonoBehaviour, IClientHandlerGuid
    {
        public void OnConnected(Guid matchId)
        {
            GameController.Instance.StartGame();
        }
        public void OnMatchJoined(Guid matchId)
        {
            GameController.Instance.StartGame();
        }

        public void OnMatchJoinedFailed(string errorMessage)
        {
        }

        public void OnMatchEnded(Guid matchId)
        {
        }
    }
}