using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Network;
using UnityEngine.SceneManagement;
using Entities;

public class Create : MonoBehaviour {
    public InputField input;

    public void OnClick(Button btn)
    {
        btn.interactable = false;

        // 名前指定してユーザデータを登録する
        Protocol.Send(new CreateUserSend { name = input.text }, (r) =>
        {
            Entity.Instance.UpdateUserState(r.userState);
            // プロローグへ
            switch (r.userState.createStep)
            {
                case Entities.UserCreateStep.Prologue:
                    SceneManager.LoadScene(SceneName.Prologue);
                    break;
                case Entities.UserCreateStep.Created:
                    SceneManager.LoadScene(SceneName.Home);
                    break;
            }
        });
    }
    public void OnEndEdit(Button btn)
    {
        btn.interactable = !string.IsNullOrEmpty(input.text);
    }
}
