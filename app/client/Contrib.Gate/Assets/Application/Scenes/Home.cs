using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entities;
using UnityEngine.UI;
using Event;
using UI;
using Effect;

public class Home : MonoBehaviour {

    public Text userName;
    public Text coin;

	// Use this for initialization
	IEnumerator Start () {
        enabled = false;
        yield return Entity.Instance.GetInventory();
        yield return Entity.Instance.GetEggList();
        yield return Entity.Instance.GetPetList();
        yield return Entity.Instance.GetHatchList();
        yield return Entity.Instance.GetUnitList();

        Observer.Instance.Subscribe(UserState.Update, UpdateUserState);
        UpdateUserState();
        enabled = true;

        /// 試しにエフェクトを再生する
        //EffectManager.Instance.Play("Particle01", Vector3.zero, () => Debug.Log("エフェクト再生完了"));

        EffectWindow.Instance.Play();
    }

    private void OnDestroy()
    {
        Observer.Instance.Unsubscribe(UserState.Update, UpdateUserState);
    }
    // Update is called once per frame
    void Update () {
		
	}

    public void OnClickVending()
    {
        Window.Open<VendingWindow>();
    }

    public void OnClickBake()
    {
        Window.Open<BakeWindow>();
    }

    public void OnClickInventory()
    {
        Window.Open<InventoryWindow>();
    }

    public void OnClickStage()
    {
        Util.CoroutineRunner.Run(Entity.Instance.GetStageList(), ()=>
        {
            Window.Open<StageWindow>();
        });
    }
    public void OnClickEgg()
    {
        Window.Open<EggWindow>();
    }
    public void OnClickPet()
    {
        Window.Open<PetWindow>();
    }

    void UpdateUserState(string name = "", object arg = null)
    {
        var state = Entity.Instance.UserState;
        coin.text = state.coin.ToString();
        userName.text = state.playerName;
    }
}

