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
        if (!enabled) return;
        Window.Open<VendingWindow>();
    }

    public void OnClickBake()
    {
        if (!enabled) return;
        Window.Open<BakeWindow>();
    }

    public void OnClickInventory()
    {
        if (!enabled) return;
        Window.Open<InventoryWindow>();
    }

    public void OnClickStage()
    {
        if (!enabled) return;
        Util.CoroutineRunner.Run(Entity.Instance.GetStageList(), ()=>
        {
            Window.Open<StageWindow>();
        });
    }
    public void OnClickEgg()
    {
        if (!enabled) return;
        Window.Open<EggWindow>();
    }
    public void OnClickPet()
    {
        if (!enabled) return;
        Window.Open<PetWindow>();
    }

    void UpdateUserState(string name = "", object arg = null)
    {
        var state = Entity.Instance.UserState;
        coin.text = state.coin.ToString();
        userName.text = state.playerName;
    }
}

