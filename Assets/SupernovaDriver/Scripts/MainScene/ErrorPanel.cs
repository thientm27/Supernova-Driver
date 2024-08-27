using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ErrorPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI errorMessageText;
    [SerializeField] private Button tryAgainButton;

    private void Awake()
    {
        tryAgainButton.onClick.AddListener(OnButtonClick);
    }
    private void OnButtonClick()
    {
        Hide();
    }
    public void Display(string errorMessage)
    {
        Debug.LogError(errorMessage);

        gameObject.SetActive(true);
        errorMessageText.text = errorMessage;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
