using Elympics;
using UnityEngine;
using UnityEngine.UI;
using System;
using JetBrains.Annotations;
using Elympics.Models.Authentication;

public class MatchmakingManager : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private ErrorPanel errorPanel;
    [SerializeField] private GameObject throbber;

    private void Start()
    {
        if (ElympicsLobbyClient.Instance == null)
            return;

        ControlPlayAccess(ElympicsLobbyClient.Instance.IsAuthenticated);
        ElympicsLobbyClient.Instance.AuthenticationSucceeded += OnAuthenticationSucceeded;

        ElympicsLobbyClient.Instance.Matchmaker.MatchmakingFailed += OnMatchmakingFailed;
        //ElympicsLobbyClient.Instance.RoomsManager.ma
    }

    private void OnDestroy()
    {
        if (ElympicsLobbyClient.Instance == null)
            return;

        ElympicsLobbyClient.Instance.AuthenticationSucceeded -= OnAuthenticationSucceeded;
        ElympicsLobbyClient.Instance.Matchmaker.MatchmakingFailed -= OnMatchmakingFailed;
    }
    private void OnAuthenticationSucceeded(AuthData authData)
    {
        ControlPlayAccess(true);
    }

    /// <summary>
    /// Set Play button interactable
    /// </summary>
    private void ControlPlayAccess(bool allowToPlay)
    {
        if (playButton is not null) playButton.interactable = allowToPlay;
    }
    private void OnMatchmakingFailed((string error, Guid _) result)
    {
        errorPanel.Display(result.error);
        throbber.gameObject.SetActive(false);
        ControlPlayAccess(true);
    }

    [UsedImplicitly]
    public async void PlayOnline()
    {
        if (ElympicsLobbyClient.Instance == null)
        {
            Debug.LogWarning("In order for this method to work you need to start from the menu scene. Method call skipped.");
            return;
        }

        throbber.gameObject.SetActive(true);
        ControlPlayAccess(false);

        await ElympicsLobbyClient.Instance.RoomsManager.StartQuickMatch(ElympicsAuthenticateHandler.Instance.QueueName); //Start match with propper queue name, start Gameplay scene after success

        ElympicsAuthenticateHandler.Instance.ResetWalletQueue();
    }
}
