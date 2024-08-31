using System;
using Elympics;

namespace SupernovaDriver.Scripts.SceneController.Game
{
    public class GameClientHandler : ElympicsMonoBehaviour, IClientHandlerGuid
    {
  
        public void OnMatchJoined(Guid matchId)
        {
           GameController.Instance.StartGame(true);
        }

        public void OnMatchJoinedFailed(string errorMessage)
        {
        }

        public void OnMatchEnded(Guid matchId)
        {
     
        }
        
        
    }
}