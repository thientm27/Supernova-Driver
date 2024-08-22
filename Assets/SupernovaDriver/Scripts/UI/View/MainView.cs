using Imba.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class MainView : UIView
{
    [SerializeField] private TMP_Text txtLevel;
    [SerializeField] private TMP_Text txtMoney;

    public UnityEvent OnClickPlay;
    public UnityEvent OnClickSetting;

    public void BTN_Play()
    {
        OnClickPlay?.Invoke();
    }    

    public void BTN_Setting()
    {
        OnClickSetting?.Invoke();
    }    
}
