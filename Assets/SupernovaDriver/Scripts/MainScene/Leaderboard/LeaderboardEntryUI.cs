using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Elympics;
using WebSocketSharp;


public class LeaderboardEntryUI : MonoBehaviour
{
    [Header("UI references")]
    [SerializeField] private GameObject top1;
    [SerializeField] private GameObject top2;
    [SerializeField] private GameObject top3;
    [SerializeField] private GameObject topText;
    [SerializeField] private TextMeshProUGUI positionText;
    [SerializeField] private TextMeshProUGUI nicknameText;
    [SerializeField] private TextMeshProUGUI scoreText;

    [SerializeField] private Color highlightColor;
    [SerializeField] private Color defaultColor;

    public void SetValues(LeaderboardEntry entry)
    {
        gameObject.SetActive(true);

        if (entry == null)
            return;

        top1.SetActive(entry.Position == 1);
        top2.SetActive(entry.Position == 2);
        top3.SetActive(entry.Position == 3);
        topText.SetActive(entry.Position > 3);
        positionText.text = entry.Position.ToString() + ".";
        nicknameText.text = entry.Nickname.IsNullOrEmpty()? entry.UserId : entry.Nickname;
        scoreText.text = entry.Score.ToString("0");

        ChangeEntryToDefaultStyle();
    }
    private void ChangeEntryToDefaultStyle()
    {
        ChangeTextToDefault(positionText);
        ChangeTextToDefault(nicknameText);
        ChangeTextToDefault(scoreText);
    }
    public void HighlightEntry()
    {
        ChangeTextToHighlight(positionText);
        ChangeTextToHighlight(nicknameText);
        ChangeTextToHighlight(scoreText);
    }

    private void ChangeTextToHighlight(TextMeshProUGUI text)
    {
        text.color = highlightColor;
        text.fontStyle = FontStyles.Bold;
    }
    private void ChangeTextToDefault(TextMeshProUGUI text)
    {
        text.color = defaultColor;
        text.fontStyle = FontStyles.Normal;
    }
}
