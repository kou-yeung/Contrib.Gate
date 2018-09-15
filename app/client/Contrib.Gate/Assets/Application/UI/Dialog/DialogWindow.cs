using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace UI
{
    public class DialogWindow : Window
    {
        public Button Yes;
        public Button No;
        public Button Ok;
        public Text title;
        public Text message;

        private Action OnYes;
        private Action OnNo;
        private Action OnOk;

        public static void OpenYesNo(string title, string message, Action OnYes = null, Action OnNo = null)
        {
            var window = Open<DialogWindow>();
            window.title.text = title;
            window.message.text = message;
            window.Ok.gameObject.SetActive(false);
            window.No.gameObject.SetActive(true);
            window.Yes.gameObject.SetActive(true);

            window.OnYes = OnYes;
            window.OnNo = OnNo;
        }
        public static void OpenOk(string title, string message, Action OnOk = null)
        {
            var window = Open<DialogWindow>();
            window.title.text = title;
            window.message.text = message;
            window.Ok.gameObject.SetActive(true);
            window.No.gameObject.SetActive(false);
            window.Yes.gameObject.SetActive(false);

            window.OnOk = OnOk;
        }

        public override void OnButtonClick(Button btn)
        {
            switch (btn.name)
            {
                case "YesButton":
                    if (OnYes != null) OnYes();
                    Close();
                    break;
                case "NoButton":
                    if (OnNo != null) OnNo();
                    Close();
                    break;
                case "OKButton":
                    if (OnOk != null) OnOk();
                    Close();
                    break;
                default:
                    base.OnButtonClick(btn);
                    break;
            }
        }

    }
}
