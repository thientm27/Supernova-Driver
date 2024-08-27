using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Elympics.Models.Authentication;
using ElympicsLobbyPackage.Authorization;
using ElympicsLobbyPackage.Session;
using ElympicsLobbyPackage;
using Elympics;

public class LobbyUIManager : MonoBehaviour
{
    [SerializeField] private Image playerAvatar;
    [SerializeField] private TextMeshProUGUI playerNickname;
    [SerializeField] private TextMeshProUGUI playerEthAddress;

    [SerializeField] private List<(TextMeshProUGUI, TextMeshProUGUI)> leaderboardContent = new List<(TextMeshProUGUI, TextMeshProUGUI)>();
    [SerializeField] private TextMeshProUGUI leaderboardTimer;
    [SerializeField] private TextMeshProUGUI playButtonText;

    [SerializeField] private GameObject connectWalletButton;
    [SerializeField] private GameObject authenticationInProgressScreen;
    [SerializeField] private GameObject matchmakingInProgressScreen;
    [SerializeField] private GameObject errorScreen;
    [SerializeField] private TextMeshProUGUI errorMessage;

    private string playQueue = null;
    private string leaderboardQueue = null;
    private LeaderboardClient leaderboardClient = null;
    private PersistentLobbyManager persistentLobbyManager = null;
    public void SetPersistentLobbyManager(PersistentLobbyManager newValue) => persistentLobbyManager = newValue;

    public void SetAuthenticationScreenActive(bool newValue) => authenticationInProgressScreen.SetActive(newValue);

    public void SetLobbyUIVariant(SessionManager sessionManager)
    {
        // gathering info for further evaluation
        Capabilities capabilities = sessionManager.CurrentSession.Value.Capabilities;
        var currentAuthType = ElympicsLobbyClient.Instance.AuthData.AuthType;
        bool isGuest = currentAuthType is AuthType.ClientSecret or AuthType.None;

        // UI elements adjustments logic
        playButtonText.text = isGuest ? "Train now" : "Play now";
        playerAvatar.gameObject.SetActive(!isGuest);
        playerNickname.gameObject.SetActive(!isGuest);
        if (!isGuest)
        {
            playerNickname.text = sessionManager.CurrentSession.Value.AuthData.Nickname;

        }
        playerEthAddress.gameObject.SetActive(!isGuest && !capabilities.IsTelegram());
        connectWalletButton.SetActive(capabilities.IsEth() || capabilities.IsTon() && isGuest);


        // adjusting queues
        playQueue = currentAuthType switch
        {
            AuthType.Telegram => "telegram",
            AuthType.EthAddress => "eth",
            _ => "training",
        };

        leaderboardQueue = currentAuthType == AuthType.Telegram ? "telegram" : "eth";

        CreateLeaderboardClient();
        FetchLeaderboardEntries();
    }
    public void CreateLeaderboardClient()
    {
        var pageSize = 5; // Depends on display design - in this game we show top 5
        var gameVersion = LeaderboardGameVersion.All; // Worth changing to Current if new version contains important balance changes
        var leaderboardType = LeaderboardType.BestResult; // Adjust to the type of game
        var customTimeScopeFrom = "2023-07-07T12:00:00+02:00";
        var customTimeScopeTo = "2023-07-14T12:00:00+02:00";

        var timeScopeObject = new LeaderboardTimeScope(DateTimeOffset.Parse(customTimeScopeFrom), DateTimeOffset.Parse(customTimeScopeTo));
        leaderboardClient = new LeaderboardClient(pageSize, timeScopeObject, leaderboardQueue, gameVersion, leaderboardType);
    }


    public void FetchLeaderboardEntries() => leaderboardClient.FetchFirstPage(DisplayTop5Entries);

    private void DisplayTop5Entries(LeaderboardFetchResult result)
    {
        //reseting leaderboard
        foreach (var leaderboardRow in leaderboardContent)
        {
            leaderboardRow.Item1.text = "";
            leaderboardRow.Item2.text = "";
        }

        for (int i = 0; i < 5 && i < result.Entries.Count; i++)
        {
            leaderboardContent[i].Item1.text = result.Entries[i].Nickname;
            leaderboardContent[i].Item2.text = result.Entries[i].Score.ToString();
        }
    }
    public void ConnectToWallet()
    {
        persistentLobbyManager.ConnectToWallet();
        authenticationInProgressScreen.SetActive(true);
    }
    public void PlayGame()
    {
        ElympicsLobbyClient.Instance.RoomsManager.StartQuickMatch(playQueue);
        persistentLobbyManager.SetAppState(PersistentLobbyManager.AppState.Matchmaking);
        matchmakingInProgressScreen.SetActive(true);
    }
}