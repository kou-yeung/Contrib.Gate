using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Network;
using UnityEngine.SceneManagement;

public class Prologue : MonoBehaviour {

    public void OnClick(Button btn)
    {
        // プロローグを観たと通信する!!
        Protocol.Send(new FinishPrologueSend(), r =>
        {
            switch (r.step)
            {
                case Entities.UserCreateStep.Created:
                    SceneManager.LoadScene(SceneName.Home);
                    break;
            }
        });

    }
}
