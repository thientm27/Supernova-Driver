using Elympics;
using Elympics.Models.Authentication;
using UnityEngine;

namespace Services
{
    public class ElympicsService : MonoBehaviour
    {
        public ElympicsService()
        {
            ElympicsLobbyClient.Instance.AuthenticationSucceeded += HandleAuthenticated;
        }

        private void HandleAuthenticated(AuthData result)
        {
            Debug.Log("User authenticated - can start using LeaderboardClient");
            //FetchFirst();
        }
    }
}