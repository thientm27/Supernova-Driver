using System;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using Elympics.Models.Authentication;
using ElympicsLobbyPackage.Authorization;
using ElympicsLobbyPackage.Session;
using ElympicsLobbyPackage;
using Elympics;
using UnityEngine;

public class LobbyUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerNickName;
    [SerializeField] private TextMeshProUGUI playerEthAddess;

    [SerializeField] private List<(TextMeshProUGUI, TextMeshProUGUI)> leaderBoardContent = new List<(TextMeshProUGUI, TextMeshProUGUI)>();
    [SerializeField] private TextMeshProUGUI playButtonText;

    [SerializeField] private GameObject connectWalletButton;
    [SerializeField] private GameObject authenticationInProgressScreen;
    [SerializeField] private GameObject matchMakingInProgressScreen;
    [SerializeField] private GameObject playerAvatar;

    [SerializeField] private LeaderboardsDisplayer leaderboardsDisplayer;

    private PersistentLobbyManager persistentLobbyManager = null;
    private string playQueue = "training";
    private string leaderboardQueue;
    public void SetPersistentLobbyManager(PersistentLobbyManager newValue) => persistentLobbyManager = newValue;

    public void SetAuthenticationScreenActive(bool newValue) => authenticationInProgressScreen.SetActive(newValue);

    public void SetLobbyVariantUI(SessionManager sessionManager)
    {
        Capabilities capabilities = sessionManager.CurrentSession.Value.Capabilities;
        var currentAthType = ElympicsLobbyClient.Instance.AuthData.AuthType;
        bool isGuest = currentAthType is AuthType.ClientSecret or AuthType.None;

        playButtonText.text = isGuest ? "Train now" : "Play now";
        playerAvatar.SetActive(!isGuest);
        playerNickName.gameObject.SetActive(!isGuest);
        if (!isGuest)
        {
            playerNickName.text = sessionManager.CurrentSession.Value.AuthData.Nickname;
            if (!capabilities.IsTelegram())
            {
                playerEthAddess.text = sessionManager.CurrentSession.Value.SignWallet;
            }
        }
        playerEthAddess.gameObject.SetActive(!isGuest && !capabilities.IsTelegram());
        connectWalletButton.SetActive((capabilities.IsEth() || capabilities.IsTon()) && !isGuest);
        playQueue = currentAthType switch
        {
            AuthType.Telegram => "telegram",
            AuthType.EthAddress => "eth",
            _ => "training",
        };

        leaderboardQueue = currentAthType == AuthType.Telegram ? "telegram" : "eth";

        CreateLeaderboardClient();
    }
    public void CreateLeaderboardClient()
    {
        leaderboardsDisplayer.InitializeAndRun();
    }
    public void ConnectToWallet()
    {
        persistentLobbyManager.ConnectToWallet();
        authenticationInProgressScreen.SetActive(true);
    }

    async public void PlayGame()
    {
        persistentLobbyManager.SetAppState(PersistentLobbyManager.AppState.MatchMaking);
        matchMakingInProgressScreen.SetActive(true);
        await ElympicsLobbyClient.Instance.RoomsManager.StartQuickMatch(playQueue);
    }

    public void ShowAccountInfo()
    {
        ElympicsExternalCommunicator.Instance.WalletCommunicator.ExternalShowAccountInfo();
    }
}
