using UnityEngine;
using ElympicsLobbyPackage.Blockchain.Wallet;
using ElympicsLobbyPackage.Session;
using ElympicsLobbyPackage;
using Elympics;
using Cysharp.Threading.Tasks;
using System;

public class PersistentLobbyManager : MonoBehaviour
{
    private SessionManager sessionManager = null;
    private Web3Wallet web3Wallet = null;
    private LobbyUIManager lobbyUIManager = null;

    private void Start()
    {
        GameObject elympicsExternalCommunicator = ElympicsExternalCommunicator.Instance.gameObject;
        sessionManager = elympicsExternalCommunicator.GetComponent<SessionManager>();
        web3Wallet = elympicsExternalCommunicator.GetComponent<Web3Wallet>();
        SetLobbyUIManager();

        ElympicsExternalCommunicator.Instance.GameStatusCommunicator.ApplicationInitialized();
        AttemptStartAuthenticate().Forget();
    }

    private void SetLobbyUIManager()
    {
        lobbyUIManager = FindObjectOfType<LobbyUIManager>();
        lobbyUIManager.SetPersistentLobbyManager(this);
    }
    private async UniTask AttemptStartAuthenticate()
    {
        lobbyUIManager.SetAuthenticationScreenActive(true);
        if (!ElympicsLobbyClient.Instance.IsAuthenticated || ElympicsLobbyClient.Instance.WebSocketSession.IsConnected)
        {
            await sessionManager.AuthenticateFromExternalAndConnect();
        }
        lobbyUIManager.SetAuthenticationScreenActive(false);

        lobbyUIManager.SetLobbyUIVariant(sessionManager);
        web3Wallet.WalletConnectionUpdated += ReactToAuthenticationChange;
    }
    public void ReactToAuthenticationChange(WalletConnectionStatus status)
    {
        if (appState == AppState.Lobby)
        {
            AttemptReAuthenticate();
        }
    }
    public async void AttemptReAuthenticate()
    {
        await sessionManager.TryReAuthenticateIfWalletChanged();
        lobbyUIManager.SetLobbyUIVariant(sessionManager);
        lobbyUIManager.SetAuthenticationScreenActive(false);
    }
    public async void ConnectToWallet()
    {
        await sessionManager.ConnectToWallet();
    }
    public enum AppState { Lobby, Matchmaking, Gameplay }
    private AppState appState = AppState.Lobby;
    public void SetAppState(AppState newState)
    {
        appState = newState;
        if (appState == AppState.Lobby) AttemptReAuthenticate();
    }
}