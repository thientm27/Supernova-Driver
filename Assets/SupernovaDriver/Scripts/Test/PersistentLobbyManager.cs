using UnityEngine;
using ElympicsLobbyPackage.Blockchain.Wallet;
using ElympicsLobbyPackage.Session;
using ElympicsLobbyPackage;
using Elympics;
using Cysharp.Threading.Tasks;

public class PersistentLobbyManager : MonoBehaviour
{
    private SessionManager sessionManager;
    private Web3Wallet web3Wallet;
    private LobbyUIManager lobbyUIManager;

    public enum AppState
    {
        Lobby, MatchMaking, Gameplau
    }
    private AppState appState = AppState.Lobby;
    public void SetAppState(AppState newState)
    {
        appState = newState;
        if (appState == AppState.Lobby)
        {
            SetLobbyUIManager();
            AttempReAuthenticate();
        }
    }
    private void SetLobbyUIManager()
    {
        lobbyUIManager = FindObjectOfType<LobbyUIManager>();
        lobbyUIManager.SetPersistentLobbyManager(this);
    }

    private void Start()
    {
        GameObject elympicsExternalCommunicator = ElympicsExternalCommunicator.Instance.gameObject;
        sessionManager = elympicsExternalCommunicator.GetComponent<SessionManager>();
        web3Wallet = elympicsExternalCommunicator.GetComponent<Web3Wallet>();

        SetLobbyUIManager();

        ElympicsExternalCommunicator.Instance.GameStatusCommunicator.ApplicationInitialized();
        AttemptStartAuthenticate().Forget();
    }

    private async UniTask AttemptStartAuthenticate()
    {
        lobbyUIManager.SetAuthenticationScreenActive(true);
        if(!ElympicsLobbyClient.Instance.IsAuthenticated || !ElympicsLobbyClient.Instance.WebSocketSession.IsConnected)
        {
            await sessionManager.AuthenticateFromExternalAndConnect();
        }
        lobbyUIManager.SetAuthenticationScreenActive(false);

        lobbyUIManager.SetLobbyVariantUI(sessionManager);
        web3Wallet.WalletConnectionUpdated += ReactToAuthenticationChange;
    }

    private async void AttempReAuthenticate()
    {
        await sessionManager.TryReAuthenticateIfWalletChanged();
        lobbyUIManager.SetLobbyVariantUI(sessionManager);
        lobbyUIManager.SetAuthenticationScreenActive(false);
    }

    public void ReactToAuthenticationChange(WalletConnectionStatus status)
    {
        if(appState == AppState.Lobby)
        {
            AttempReAuthenticate();
        }
    }

    public async void ConnectToWallet()
    {
        await sessionManager.ConnectToWallet();
    }
}
