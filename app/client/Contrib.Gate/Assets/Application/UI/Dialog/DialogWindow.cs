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

        /// <summary>
        /// 選択肢で開く
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="choice"></param>
        /// <param name="OnChoice"></param>
        public static void OpenChoice(string title, string message, string[] choice, Action<string> OnChoice)
        {
            var window = Open<DialogWindow>();
            window.title.text = title;
            window.message.text = message;
            window.Ok.gameObject.SetActive(false);
            window.No.gameObject.SetActive(true);
            window.Yes.gameObject.SetActive(true);

            window.Yes.GetComponentInChildren<Text>().text = choice[0];
            window.OnYes = ()=> { OnChoice(choice[0]); };

            window.No.GetComponentInChildren<Text>().text = choice[1];
            window.OnNo = () => { OnChoice(choice[1]); };
        }

        protected override void OnButtonClick(Button btn)
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
