using UnityEngine;
using Elympics;
using ElympicsLobbyPackage.Authorization;
using ElympicsLobbyPackage.Blockchain.Wallet;
using WebSocketSharp;
using Elympics.Models.Authentication;
using ElympicsLobbyPackage;
using System;
using Cysharp.Threading.Tasks;

public class LoginManager : MonoBehaviour
{
    [SerializeField] private LeaderboardsDisplayer leaderboardsDisplayer;
    [SerializeField] private MainMenuView mainMenuView;
    [SerializeField] private GameObject throbber;
    [SerializeField] private MainMenuDisconnectedPanel mainMenuDisconnectedPanel;

    private Web3Wallet web3Wallet;

    private void Start()
    {
        web3Wallet = ElympicsLobbyClient.Instance.gameObject.GetComponent<Web3Wallet>();

        ElympicsAuthenticateHandler.Instance.InitializationComplete += ConnectToElympics;
        //ElympicsExternalCommunicator.Instance.WalletCommunicator.WalletConnected += OnWalletConnected;
        //ElympicsExternalCommunicator.Instance.WalletCommunicator.WalletDisconnected += OnWalletDisconnected;

        // Check if user has connected/disconnected during gameplay
        if (ElympicsAuthenticateHandler.Instance.WalletConnectedQueued) OnWalletConnected(null, null);
        else if (ElympicsAuthenticateHandler.Instance.WalletDisconnectedQueued) OnWalletDisconnected();


#if UNITY_WEBGL && !UNITY_EDITOR
        if (ElympicsLobbyClient.Instance.WebSocketSession.IsConnected)
        {
            throbber.SetActive(false);
            AdjustToUserAuthentication();
        }
#else
        if (ElympicsLobbyClient.Instance.WebSocketSession.IsConnected)
        {
            Debug.Log("### websocket connect");
            throbber.SetActive(false);
            AdjustToUserAuthentication();
        }
        else
        {
            Debug.Log("### websocket not-connect");
            ConnectAsGuest().Forget();
            AdjustToUserAuthentication();
        }
#endif

    }

    private void OnDisable()
    {
        ElympicsAuthenticateHandler.Instance.InitializationComplete -= ConnectToElympics;
        //ElympicsExternalCommunicator.Instance.WalletCommunicator.WalletConnected -= OnWalletConnected;
        //ElympicsExternalCommunicator.Instance.WalletCommunicator.WalletDisconnected -= OnWalletDisconnected;
    }

    public void ShowThrobber()
    {
        throbber.SetActive(true);
    }

    /// <summary>
    /// SignOut and connect with Eth Adress
    /// </summary>
    private void OnWalletConnected(string address, string chainId)
    {
        Debug.Log("###Elympics### OnWalletConnected");

        mainMenuDisconnectedPanel.WalletRelatedOperations();

        ElympicsLobbyClient.Instance.SignOut();
        throbber.SetActive(true);
        ConnectWithEthAdress().Forget();
        ElympicsAuthenticateHandler.Instance.ResetWalletQueue();
    }

    /// <summary>
    /// SignOut and connect as a guest
    /// </summary>
    private void OnWalletDisconnected()
    {
        Debug.Log("###Elympics### OnWalletDisconnected");

        mainMenuDisconnectedPanel.WalletRelatedOperations();

        ElympicsLobbyClient.Instance.SignOut();
        throbber.SetActive(true);
        ConnectAsGuest().Forget();
        ElympicsAuthenticateHandler.Instance.ResetWalletQueue();
    }

    /// <summary>
    /// Connect to Elympics based on ExternalAuthData
    /// </summary>
    private async void ConnectToElympics(ExternalAuthData authData)
    {
        Debug.Log("###Elympics### InitializationComplete - Connecting with authData");

        if (authData.AuthData is not null)
        {
            await ConnectWithCachedAuthData(authData);
        }
        else if (authData.Capabilities.IsEth() || authData.Capabilities.IsTon())
        {
            await HandleWeb3Connection();
        }
        else await ConnectAsGuest();
    }

    /// <summary>
    /// Method to handle Web3 connection
    /// </summary>
    private async UniTask HandleWeb3Connection()
    {
        try
        {
            web3Wallet ??= ElympicsLobbyClient.Instance.gameObject.GetComponent<Web3Wallet>();

            string walletAddress = await web3Wallet.ConnectWeb3();
            if (walletAddress.IsNullOrEmpty())
            {
                await ConnectAsGuest();
            }
            else
            {
                await ConnectWithEthAdress();
            }
        }
        catch (Exception)
        {
            await ConnectAsGuest();
        }
    }

    /// <summary>
    /// Connect with Cached Auth Data (AuthType.Telegram if user is connecting through Telegram), if user will reject Signature Request then connect as a guest
    /// </summary>
    private async UniTask ConnectWithCachedAuthData(ExternalAuthData authData)
    {
        Debug.Log("###Elympics### ConnectWithCachedAuthData");

        await UniTask.WaitUntil(() => ElympicsAuthenticateHandler.Instance.Region != null);

        await ElympicsLobbyClient.Instance.ConnectToElympicsAsync(new ConnectionData() { AuthFromCacheData = new CachedAuthData(authData.AuthData, true), Region = new RegionData(ElympicsAuthenticateHandler.Instance.Region)});

        if (!ElympicsLobbyClient.Instance.IsAuthenticated) // user has rejected Signature Request
        {
            ConnectAsGuest().Forget();
            return;
        }

        throbber.SetActive(false);
        mainMenuView.OnInitializationComplete();
        ElympicsAuthenticateHandler.Instance.SetQueueName();
        AdjustToUserAuthentication();
    }

    /// <summary>
    /// Connect with Eth wallet adress, if user will reject Signature Request then connect as a guest
    /// </summary>
    public async UniTask ConnectWithEthAdress()
    {
        Debug.Log("###Elympics### ConnectWithEthAdress");

        await UniTask.WaitUntil(() => ElympicsAuthenticateHandler.Instance.Region != null);

        await ElympicsLobbyClient.Instance.ConnectToElympicsAsync(new ConnectionData() { AuthType = AuthType.EthAddress, Region = new RegionData(ElympicsAuthenticateHandler.Instance.Region) });

        if (!ElympicsLobbyClient.Instance.IsAuthenticated) // user has rejected Signature Request
        {
            ConnectAsGuest().Forget();
            return;
        }

        throbber.SetActive(false);
        mainMenuView.OnInitializationComplete();
        ElympicsAuthenticateHandler.Instance.SetQueueName();
        AdjustToUserAuthentication();
    }

    private async UniTask ConnectAsGuest()
    {
        Debug.Log("###Elympics### ConnectAsGuest");

        await UniTask.WaitUntil(() => ElympicsAuthenticateHandler.Instance.Region != null);

        Debug.Log("###Elympics### ConnectAsGuest 111");
        await ElympicsLobbyClient.Instance.ConnectToElympicsAsync(new ConnectionData() { AuthType = AuthType.ClientSecret, Region = new RegionData(ElympicsAuthenticateHandler.Instance.Region) });
        Debug.Log("###Elympics### ConnectAsGuest 222");
        throbber.SetActive(false);
        mainMenuView.OnInitializationComplete();
        ElympicsAuthenticateHandler.Instance.SetQueueName();
        AdjustToUserAuthentication();
    }

    /// <summary>
    /// Initialize Leaderboard
    /// </summary>
    private void AdjustToUserAuthentication()
    {
        Debug.Log("###Elympics### AdjustToUserAuthentication");

        if (!leaderboardsDisplayer.FetchAlreadyStarted)
            leaderboardsDisplayer.InitializeAndRun();
    }
}
