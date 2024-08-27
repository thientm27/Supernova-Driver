using ElympicsLobbyPackage;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserDataPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nicknameText;
    [SerializeField] private TextMeshProUGUI walletAdressText;
    //[SerializeField] private RectTransform rect;
    [SerializeField] private Button button;

    private void Awake()
    {
        button.onClick.AddListener(onPlayerDataPanelClick);
    }

    private void onPlayerDataPanelClick()
    {
        ElympicsExternalCommunicator.Instance.WalletCommunicator.ExternalShowAccountInfo();
    }

    public void HideUserDataPanel()
    {
        gameObject.SetActive(false);
    }
    public void SetDataAndShowPanel(Elympics.Models.Authentication.AuthData authData)
    {
        nicknameText.text = authData.Nickname;
        //SetWalletText(authData.ETHAddress());
        gameObject.SetActive(true);

        //LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
    }

    public void SetWalletText(string fullString)
    {
        string truncatedString = TruncateMiddle(fullString, 5, 3); //  keep 5 characters at start, 3 at end
        walletAdressText.text = truncatedString;
    }

    // Truncate the middle of a string and add ellipsis
    public string TruncateMiddle(string input, int keepStart, int keepEnd)
    {
        if (input.Length <= keepStart + keepEnd)
        {
            return input;
        }

        string start = input.Substring(0, keepStart);
        string end = input.Substring(input.Length - keepEnd);

        return $"{start}...{end}";
    }


}
