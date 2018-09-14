using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Network;
using Util.Time;
using Security;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public void OnClick(Button btn)
    {
        btn.interactable = false;

        // ログインする
        Protocol.Send(new LoginSend(), (r) =>
        {
            Crypt.Init(r.timestamp, r.iv, r.key);

            ServerTime.Init(r.timestamp);
            switch (r.step)
            {
                case Entities.UserCreateStep.EnterName:
                    // ユーザ作成画面へ
                    SceneManager.LoadScene(SceneName.Create);
                    break;
                case Entities.UserCreateStep.Prologue:
                    // プロローグ画面へ
                    SceneManager.LoadScene(SceneName.Prologue);
                    break;
                case Entities.UserCreateStep.Created:
                    // 作成済なので、ホーム画面へ
                    SceneManager.LoadScene(SceneName.Home);
                    break;
            }
        });
    }
}
