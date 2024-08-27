using UnityEngine;
using Elympics;
using UnityEngine.UI;

public class MainMenuDisconnectedPanel : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Button button;

    private bool isWalletRelatedOperations;

    private void Awake()
    {
        button.onClick.AddListener(ButtonClick);
    }
    private void Start()
    {
        //ElympicsLobbyClient.Instance.WebSocketSession.Disconnected += WebSocketSession_Disconnected;
    }

    private void OnDisable()
    {
        //ElympicsLobbyClient.Instance.WebSocketSession.Disconnected -= WebSocketSession_Disconnected;
    }

    /// <summary>
    ///  Set flag to make sure that disconnected popup doesn't show while connecting/disconnecting with wallet
    /// </summary>
    public void WalletRelatedOperations()
    {
        isWalletRelatedOperations = true;
    }


    private void ButtonClick()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
             Application.ExternalEval("document.location.reload(true)");
#endif
    }


    private void WebSocketSession_Disconnected()
    {
        if (!isWalletRelatedOperations) panel.SetActive(true);
        else isWalletRelatedOperations = false;
    }

}
