using System;
using System.Collections.Generic;
using Elympics;
using Elympics.Models.Authentication;
using JetBrains.Annotations;
using UnityEngine;

public class LeaderboardTester : MonoBehaviour
{
    [SerializeField] private int pageSize = 5;
    [SerializeField] private string queue;
    [SerializeField] private LeaderboardGameVersion gameVersion = LeaderboardGameVersion.All;
    [SerializeField] private LeaderboardType leaderboardType = LeaderboardType.BestResult;
    [SerializeField] private LeaderboardTimeScopeType timeScope = LeaderboardTimeScopeType.Month;
    [SerializeField] private string customTimeScopeFrom = "2023-05-22T12:00:00+02:00";
    [SerializeField] private string customTimeScopeTo = "2023-05-23T12:00:00+02:00";

    private LeaderboardClient _leaderboardClient;

    private void Start()
    {
        CreateLeaderboardClient();

        ElympicsLobbyClient.Instance.AuthenticationSucceeded += HandleAuthenticated;
    }

    private void HandleAuthenticated(AuthData result)
    {
        Debug.Log("User authenticated - can start using LeaderboardClient");
        FetchFirst();
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
    [UsedImplicitly] private void FetchUserPage() => _leaderboardClient.FetchPageWithUser(DisplayEntries);
    [UsedImplicitly] private void FetchNext() => _leaderboardClient.FetchNextPage(DisplayEntries);
    [UsedImplicitly] private void FetchPrevious() => _leaderboardClient.FetchPreviousPage(DisplayEntries);
    [UsedImplicitly] private void FetchCurrent() => _leaderboardClient.FetchRefreshedCurrentPage(DisplayEntries);

    private void DisplayEntries(LeaderboardFetchResult result)
    {
        var totalPages = (int)Math.Ceiling(result.TotalRecords / (float)pageSize);
        Debug.Log($"Fetched leaderboards: {result.Entries.Count} entries, page {result.PageNumber} of {totalPages}");
        foreach (var entry in result.Entries)
            Debug.Log($"{entry.Position}. Score: {entry.Score} User: {entry.UserId} When: {entry.ScoredAt?.LocalDateTime} MatchId: {entry.MatchId} TournamentId: {entry.TournamentId} NickName: {entry.Nickname}");
    }
    //[UsedImplicitly]
    //private void UpdatePlayerScore(int score)
    //{
    //    var playerId = ElympicsLobbyClient.Instance.UserGuid.ToString();
    //    ElympicsLeaderboard.SendScore(score, queue, gameVersion, leaderboardType, new LeaderboardTimeScope(timeScope),
    //        onSuccess: () => Debug.Log($"Score {score} sent successfully for user {playerId}"),
    //        onFailure: error => Debug.LogError($"Failed to send score for user {playerId}: {error}")
    //    );
    //}

    private static void CustomFailHandler(LeaderboardFetchError fetchError)
    {
        Debug.LogError(fetchError);

        if (fetchError == LeaderboardFetchError.NoRecords)
            Debug.Log("You need to generate any match results to be able to view them in the leaderboards");
    }
    public void SendLeaderBoard()
    {
        //Elympics.(new ResultMatchPlayerDatas(new List<ResultMatchPlayerData>
        //  {
        //    // scoreManager.Score[i] is only an example of usage - it could be any value you want to store as a result
        //    new ResultMatchPlayerData { MatchmakerData = new float[1] { scoreManager.Score[0] } },
        //    new ResultMatchPlayerData { MatchmakerData = new float[1] { scoreManager.Score[1] } },
        //  }));
    }
}
