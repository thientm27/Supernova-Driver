using Cysharp.Threading.Tasks;
using Elympics;
using Elympics.Models.Authentication;
using ElympicsLobbyPackage;
using ElympicsLobbyPackage.Authorization;
using System;
using UnityEngine;

public class ElympicsAuthenticateHandler : MonoBehaviour
{
    public static ElympicsAuthenticateHandler Instance { get; private set; }
    public string QueueName => playQueueName;
    public string Region => region;
    public string LeaderboardQueueName => leaderboardQueueName;
    public ExternalAuthData ExternalAuthData => externalAuthData;
    public bool WalletConnectedQueued => walletConnectedQueued;
    public bool WalletDisconnectedQueued => walletDisconnectedQueued;

    public event Action<ExternalAuthData> InitializationComplete;
    [SerializeField] private ElympicsConfig config;

    private string playQueueName;
    private string leaderboardQueueName;
    private ExternalAuthData externalAuthData;
    private ElympicsGameConfig currentGameConfig;
    private bool walletConnectedQueued;
    private bool walletDisconnectedQueued;
    private string region;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            // Initialize Application - Call this only once
#if UNITY_WEBGL && !UNITY_EDITOR
        ElympicsExternalCommunicator.Instance.GameStatusCommunicator.ApplicationInitialized();
#endif

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        SetCurrentGameConfig();
    }
    private void Start()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        GetAuthData().Forget();
#else
        playQueueName = Queue.Training;
#endif
        //ElympicsExternalCommunicator.Instance.WalletCommunicator.WalletConnected += OnWalletConnected;
        //ElympicsExternalCommunicator.Instance.WalletCommunicator.WalletDisconnected += OnWalletDisconnected;

        GetRegion().Forget();
    }


    private void OnDestroy()
    {
        //ElympicsExternalCommunicator.Instance.WalletCommunicator.WalletConnected -= OnWalletConnected;
        //ElympicsExternalCommunicator.Instance.WalletCommunicator.WalletDisconnected -= OnWalletDisconnected;
    }

    private async UniTask GetRegion()
    {
        var regionData = await ElympicsCloudPing.ChooseClosestRegion(ElympicsRegions.AllAvailableRegions);
        region = regionData.Region;
    }

    /// <summary>
    /// Set walletConnectedQueued and walletDisconnectedQueued flags during gameplay to execute Wallet Connection event after coming back to Main Menu
    /// </summary>
    private void OnWalletConnected(string address, string chainId)
    {
        Debug.Log("###Elympics### Wallet Connected");

        walletConnectedQueued = true;
        walletDisconnectedQueued = false;
    }

    /// <summary>
    /// Set walletConnectedQueued and walletDisconnectedQueued flags during gameplay to execute Wallet Disconnection event after coming back to Main Menu
    /// </summary>
    private void OnWalletDisconnected()
    {
        Debug.Log("###Elympics### Wallet Disconnected");

        walletConnectedQueued = false;
        walletDisconnectedQueued = true;
    }

    /// <summary>
    /// Reset walletConnectedQueued and walletDisconnectedQueued flags
    /// </summary>
    public void ResetWalletQueue()
    {
        walletConnectedQueued = false;
        walletDisconnectedQueued = false;
    }

    /// <summary>
    /// Set queue name used for matchmaking process
    /// </summary>
    public void SetQueueName()
    {
        playQueueName = ElympicsLobbyClient.Instance.AuthData.AuthType switch
        {
            AuthType.Telegram => Queue.Telegram,
            AuthType.EthAddress => Queue.Eth,
            _ => Queue.Training,
        };
    }

    /// <summary>
    /// Set queue name used for leaderboard fetching
    /// </summary>
    public void SetLeaderboardQueueName(string name)
    {
        leaderboardQueueName = name;
    }
    private void SetCurrentGameConfig()
    {
        currentGameConfig = config.GetCurrentGameConfig();
    }

    /// <summary>
    /// Send InitializationMessage and start Connecting to Elympics process in LoginManager
    /// </summary>
    private async UniTask GetAuthData()
    {
        Debug.Log("###Elympics### ElympicsExternalCommunicator InitializationMessage");
        try
        {
            //ExternalAuthData result = await ElympicsExternalCommunicator.Instance.ExternalAuthorizer.InitializationMessage(currentGameConfig.GameId, currentGameConfig.GameName, currentGameConfig.GameVersion);

            //externalAuthData = result;
            //InitializationComplete?.Invoke(externalAuthData);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Initialization failed: {ex.Message}");
        }
    }
}
