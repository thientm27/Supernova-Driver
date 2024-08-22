using Imba.UI;
using TMPro;
using UnityEngine;

namespace SupernovaDriver.Scripts.UI.Popups
{
    public class UISamplePopup : UIPopup
    {
        [SerializeField] private TextMeshProUGUI sampleTest;

        protected override void OnShowing()
        {
            base.OnShowing();
            // var parameter = (TestParam)Parameter;
            // sampleTest.text = parameter.detail;
        }

        protected override void OnHiding()
        {
            base.OnHiding();
        }

        public void OnHidePopupClick()
        {
            //UIManager.Instance.PopupManager.ShowMessageDialog("Confirm", "Are you sure to buy this item?", UIMessageBox.MessageBoxType.Yes_No,
            UIManager.Instance.PopupManager.ShowMessageDialog("Shop Purchase", "Are you sure to buy this item?", UIMessageBox.MessageBoxType.Yes_No,
                (ret) =>
                {
                    if (ret == UIMessageBox.MessageBoxAction.Accept)
                    {
                        this.Hide();
                        return true;
                    }
                    else
                    {
                        return true;
                    }
                });
        }

        public void OnCancelClick()
        {
            this.Hide();
            //UIManager.Instance.PopupManager.ShowPopup(UIPopupName.UpgradeCar);
        }
        
        public void ClosePopup()
        {
            UIManager.Instance.PopupManager.HidePopup(UIPopupName.SamplePopup);
        }
    }
}
