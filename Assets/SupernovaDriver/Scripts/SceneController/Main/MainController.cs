using System;
using Elympics;
using Services;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SupernovaDriver.Scripts.SceneController.Main
{
    public class MainController : MonoBehaviour
    {
        private ElympicsService elympicsService;

        private void Awake()
        {
            // Get services
            if (GameObject.FindGameObjectWithTag(Constants.ServicesTag) != null)
            {
                GameObject gameServiceObject = GameObject.FindGameObjectWithTag(Constants.ServicesTag);
                var gameServices = gameServiceObject.GetComponent<GameServices>();
                elympicsService = gameServices.GetService<ElympicsService>();
            }
            else
            {
                SceneManager.LoadScene(Constants.EntryScene);
            }
        }
        private void Start()
        {
            Debug.Log("Start");
            elympicsService.Initialized(() =>
            {
                Debug.Log("End");
                //SceneManager.LoadScene(Constants.GameScene);
            });
        }
        public void StartMatchMaking()
        {
            StartMatchmaking();

        }
        public async void StartMatchmaking()
        {
            if (ElympicsLobbyClient.Instance == null)
            {
                Debug.LogWarning("In order for this method to work you need to start from the menu scene. Method call skipped.");
                return;
            }
            try
            {
                var playQueue = "solo"; // Đây là tên hàng chờ mà bạn đã cấu hình

                // Bắt đầu quá trình matchmaking và tham gia vào trận đấu
                await ElympicsLobbyClient.Instance.RoomsManager.StartQuickMatch(playQueue);
                Debug.Log("Matchmaking thành công, đã tham gia vào trận đấu.");
                SceneManager.LoadScene(Constants.GameScene);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Matchmaking thất bại: {ex.Message}");
            }
        }
    }
}