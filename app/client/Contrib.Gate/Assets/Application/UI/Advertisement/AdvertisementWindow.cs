///==========================
/// 広告UI
/// Windows版 では広告表示ができないため、workaround実装します
///==========================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Network;
using Entities;
using UnityEngine.Advertisements;
using System;

namespace UI
{
    public class AdvertisementWindow : Window
    {
        public GameObject windows;
        public event Action OnCloseEvent;
        AdsBeginSend send;
        AdsBeginReceive receive;


        protected override void OnStart()
        {
            base.OnStart();
        }

        protected override void OnOpen(params object[] args)
        {
            send = new AdsBeginSend { type = (AdReward)args[0], param = (string)args[1] };

            Protocol.Send(send, (r) =>
            {
                receive = r;
#if UNITY_ADS
                // 実際に再生開始
                Advertisement.Show(new ShowOptions { resultCallback = ResultCallback });
#else
                windows.SetActive(true);
#endif
            });
            base.OnOpen(args);
        }


        /// <summary>
        /// 広告スキップした
        /// </summary>
        public void OnSkip()
        {
            ResultCallback(ShowResult.Skipped);
        }

        /// <summary>
        /// 広告観終わった
        /// </summary>
        public void OnFinished()
        {
            ResultCallback(ShowResult.Finished);
        }

        /// <summary>
        /// 広告の表示結果
        /// </summary>
        /// <param name="result"></param>
        void ResultCallback(ShowResult result)
        {
            if (result == ShowResult.Finished)
            {
                Protocol.Send(new AdsEndSend { id = receive.id }, (AdsEndReceive end) =>
                {
                    Entity.Instance.Hatchs.Modify(end.hatch);
                    Entity.Instance.Units.Modify(end.unit);
                    Close();
                });
            }
            else
            {
                Close();
            }
        }

        /// <summary>
        /// 閉じったら
        /// </summary>
        protected override void OnClose()
        {
            if (OnCloseEvent != null) OnCloseEvent();
            base.OnClose();
        }
    }
}
