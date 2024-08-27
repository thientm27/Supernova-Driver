using Cysharp.Threading.Tasks;
using Elympics;
using Elympics.Models.Authentication;
using ElympicsLobbyPackage;
using ElympicsLobbyPackage.Authorization;
using ElympicsLobbyPackage.Blockchain.Communication.Exceptions;
using ElympicsLobbyPackage.Blockchain.Wallet;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class MainMenuView : MonoBehaviour
{
    [SerializeField] private Button buttonSettings;
    //[SerializeField] private SettingsView settingsView;

    [SerializeField] private TextMeshProUGUI playButtonText;
    //[SerializeField] private Button connectWalletButton;
    //[SerializeField] private GameObject walletNotAvailablePanel;

    [SerializeField] private UserDataPanel userDataPanel;

    [SerializeField] private LoginManager loginManager;
    [SerializeField] private MainMenuDisconnectedPanel mainMenuDisconnectedPanel;

    private Web3Wallet web3Wallet;

    private void Awake()
    {
        HandlePlayerSettings();
        buttonSettings.onClick.AddListener(ShowSettingsView);
        //connectWalletButton.onClick.AddListener(ConnectToWalletButtonClick);
    }

    private void Start()
    {
        web3Wallet = ElympicsLobbyClient.Instance.gameObject.GetComponent<Web3Wallet>();

        if (ElympicsLobbyClient.Instance.WebSocketSession.IsConnected) OnInitializationComplete();
    }

    private void OnEnable()
    {
        //Settings.changedEvent += HandlePlayerSettings;
        ElympicsLobbyClient.Instance.AuthenticationSucceeded += OnAuthenticationSucceeded;
        //ElympicsExternalCommunicator.Instance.WalletCommunicator.WalletConnected += OnWalletConnected;
        //ElympicsExternalCommunicator.Instance.WalletCommunicator.WalletDisconnected += OnWalletDisconnected;
    }
    private void OnDisable()
    {
        //Settings.changedEvent -= HandlePlayerSettings;
        ElympicsLobbyClient.Instance.AuthenticationSucceeded -= OnAuthenticationSucceeded;
        //ElympicsExternalCommunicator.Instance.WalletCommunicator.WalletConnected -= OnWalletConnected;
        //ElympicsExternalCommunicator.Instance.WalletCommunicator.WalletDisconnected -= OnWalletDisconnected;
    }

    private async void ConnectToWalletButtonClick()
    {
        try
        {
            EnsureWeb3WalletInitialized();

            string walletAddress = await web3Wallet.ConnectWeb3();
            if (walletAddress.IsNullOrEmpty()) 
            {
                ShowConnectToWallet(); //if there is no wallet connected start connecting to new wallet
            }
            else
            {
                await HandleAuthenticatedWallet();
            }
        }
        catch (ResponseException e) when (e.Code == 404) 
        {
            ShowConnectToWallet(); //if error code 404 then start connecting to new wallet
        }
        catch (ResponseException)
        {
            ShowWalletNotAvailable();
        }
        catch (Exception)
        {
            ShowWalletNotAvailable();
        }
    }

    private void EnsureWeb3WalletInitialized()
    {
        web3Wallet ??= ElympicsLobbyClient.Instance.gameObject.GetComponent<Web3Wallet>();
    }

    private void ShowConnectToWallet()
    {
        ElympicsExternalCommunicator.Instance.WalletCommunicator.ExternalShowConnectToWallet();
    }

    private void ShowWalletNotAvailable()
    {
        //walletNotAvailablePanel.SetActive(true);
    }

    private async UniTask HandleAuthenticatedWallet()
    {
        //if user is authenticated already then SignOut and then connect with Eth

        if (ElympicsLobbyClient.Instance.IsAuthenticated) 
        {
            mainMenuDisconnectedPanel.WalletRelatedOperations();
            ElympicsLobbyClient.Instance.SignOut();
            loginManager.ShowThrobber();
        }
        await loginManager.ConnectWithEthAdress();
    }

    private void OnWalletConnected(string address, string chainId)
    {
        //connectWalletButton.gameObject.SetActive(false);
        playButtonText.text = "Play now";
    }
    private void OnWalletDisconnected()
    {
        //connectWalletButton.gameObject.SetActive(true);
        playButtonText.text = "Train now";
        userDataPanel.HideUserDataPanel();
    }

    private void OnAuthenticationSucceeded(AuthData authData)
    {
        OnAuthDataChanged(authData);
    }

    /// <summary>
    /// Adjust UI and queue name after initialization
    /// </summary>
    public void OnInitializationComplete()
    {
        if (ElympicsLobbyClient.Instance.AuthData is null) return;

        OnAuthDataChanged(ElympicsLobbyClient.Instance.AuthData);

        bool isGuest = ElympicsLobbyClient.Instance.AuthData.AuthType is AuthType.ClientSecret or AuthType.None;
        //connectWalletButton.gameObject.SetActive(ElympicsAuthenticateHandler.Instance.ExternalAuthData.Capabilities.IsEth() && isGuest);

        string leaderboardQueue = ElympicsAuthenticateHandler.Instance.ExternalAuthData.Capabilities.IsTelegram() ? Queue.Telegram : Queue.Eth;
        ElympicsAuthenticateHandler.Instance.SetLeaderboardQueueName(leaderboardQueue);
    }

    /// <summary>
    /// Adjust UI to AuthData
    /// </summary>
    private void OnAuthDataChanged(AuthData authData)
    {
        bool isGuest = authData.AuthType is AuthType.ClientSecret or AuthType.None;
        playButtonText.text = isGuest ? "Train now" : "Play now";
        if (!isGuest && authData.AuthType == AuthType.EthAddress) ShowUserDataPanel(authData);
    }

    /// <summary>
    /// Show user wallet data top panel
    /// </summary>
    public void ShowUserDataPanel(AuthData authData)
    {
        userDataPanel.SetDataAndShowPanel(authData);
    }

    private void ShowSettingsView()
    {
        //settingsView.Show(() => settingsView.gameObject.SetActive(false));
    }

    /// <summary>
    /// Handle audio settings from setting view
    /// </summary>
    private void HandlePlayerSettings()
    {
        //audioSource.gameObject.SetActive(Settings.MusicEnabled);
    }
}
