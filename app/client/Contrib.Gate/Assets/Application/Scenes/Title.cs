using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Network;
using Util.Time;
using Security;
using UnityEngine.SceneManagement;
using Entities;

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

            Entity.Instance.UpdateUserState(r.userState);
            switch (Entity.Instance.userState.createStep)
            {
                case UserCreateStep.EnterName:
                    // ユーザ作成画面へ
                    SceneManager.LoadScene(SceneName.Create);
                    break;
                case UserCreateStep.Prologue:
                    // プロローグ画面へ
                    SceneManager.LoadScene(SceneName.Prologue);
                    break;
                case UserCreateStep.Created:
                    // 作成済なので、ホーム画面へ
                    SceneManager.LoadScene(SceneName.Home);
                    break;
            }
        });
    }
}
