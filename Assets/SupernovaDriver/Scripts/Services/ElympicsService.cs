using System;
using Elympics;
using Elympics.Models.Authentication;
using JetBrains.Annotations;
using UnityEngine;

namespace Services
{
    public class ElympicsService
    {
        [SerializeField] private int pageSize = 5;
        [SerializeField] private string queue;
        [SerializeField] private LeaderboardGameVersion gameVersion = LeaderboardGameVersion.All;
        [SerializeField] private LeaderboardType leaderboardType = LeaderboardType.BestResult;
        [SerializeField] private LeaderboardTimeScopeType timeScope = LeaderboardTimeScopeType.Month;
        [SerializeField] private string customTimeScopeFrom = "2023-04-21T12:00:00+02:00";
        [SerializeField] private string customTimeScopeTo = "2023-04-22T12:00:00+02:00";

        private LeaderboardClient _leaderboardClient;

        private Action onInitEnded;
        public void Initialized(Action onEnded)
        {
            CreateLeaderboardClient();
            ElympicsLobbyClient.Instance.AuthenticationSucceeded += HandleAuthenticated;
            ElympicsLobbyClient.Instance.AuthenticationFailed += AuthenticattionFailed;
            onInitEnded = onEnded;
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
                var playQueue = GetPlayQueue(); // Đây là tên hàng chờ mà bạn đã cấu hình

                // Bắt đầu quá trình matchmaking và tham gia vào trận đấu
                await ElympicsLobbyClient.Instance.RoomsManager.StartQuickMatch(playQueue);
                Debug.Log("Matchmaking thành công, đã tham gia vào trận đấu.");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Matchmaking thất bại: {ex.Message}");
            }
        }
        private string GetPlayQueue()
        {
            return "solo"; // Đây là tên của hàng chờ mà bạn đã cấu hình trong Elympics Console
        }
        private void HandleAuthenticated(AuthData result)
        {
            Debug.Log("User authenticated - can start using LeaderboardClient");
            FetchFirst();
            onInitEnded?.Invoke();
        }
        private void AuthenticattionFailed(string error)
        {
            Debug.Log(error);
            Debug.Log("User authenticated - error");
        }
        [UsedImplicitly]
        private void CreateLeaderboardClient()
        {
            var timeScopeObject = timeScope != LeaderboardTimeScopeType.Custom
                ? new LeaderboardTimeScope(timeScope)
                : new LeaderboardTimeScope(DateTimeOffset.Parse(customTimeScopeFrom), DateTimeOffset.Parse(customTimeScopeTo));
            _leaderboardClient = new LeaderboardClient(pageSize, timeScopeObject, queue, gameVersion, leaderboardType);
        }
        [UsedImplicitly] private void FetchFirst() => _leaderboardClient.FetchFirstPage(DisplayEntries, CustomFailHandler);

        private void DisplayEntries(LeaderboardFetchResult result)
        {
            var totalPages = (int)Math.Ceiling(result.TotalRecords / (float)pageSize);
            Debug.Log($"Fetched leaderboards: {result.Entries.Count} entries, page {result.PageNumber} of {totalPages}");
            foreach (var entry in result.Entries)
                Debug.Log($"{entry.Position}. Score: {entry.Score} User: {entry.UserId} When: {entry.ScoredAt?.LocalDateTime} MatchId: {entry.MatchId} TournamentId: {entry.TournamentId} NickName: {entry.Nickname}");
        }

        private static void CustomFailHandler(LeaderboardFetchError fetchError)
        {
            Debug.LogError(fetchError);

            if (fetchError == LeaderboardFetchError.NoRecords)
                Debug.Log(" Fetched leaderboards You need to generate any match results to be able to view them in the leaderboards");
        }
    }
}