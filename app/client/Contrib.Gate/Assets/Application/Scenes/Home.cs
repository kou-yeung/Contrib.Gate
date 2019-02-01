using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entities;
using UnityEngine.UI;
using EventSystem;
using UI;
using Effect;
using System.Linq;

public class Home : MonoBehaviour {

    public Text userName;
    public Text coin;
    public Image image;

	// Use this for initialization
	IEnumerator Start () {
        enabled = false;
        yield return Entity.Instance.GetInventory();
        yield return Entity.Instance.GetEggList();
        yield return Entity.Instance.GetPetList();
        yield return Entity.Instance.GetHatchList();
        yield return Entity.Instance.GetUnitList();
        yield return Entity.Instance.GetBinder();

        Observer.Instance.Subscribe(UserState.Update, OnSubscribe);
        Observer.Instance.Subscribe(UnitList.UpdateEvent, OnSubscribe);
        UpdateUserState();
        enabled = true;
    }

    void OnSubscribe(string e, object o)
    {
        switch (e)
        {
            case UserState.Update:
                UpdateUserState();
                break;
            case UnitList.UpdateEvent:
                UpdateUserState();
                break;
        }
    }
    private void OnDestroy()
    {
        Observer.Instance.Unsubscribe(UserState.Update, OnSubscribe);
        Observer.Instance.Unsubscribe(UnitList.UpdateEvent, OnSubscribe);
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

        // お気に入り？ユニットの最初のキャラを設定します
        var uniqid = Entity.Instance.UnitList.items[0].uniqids.First(v => !string.IsNullOrEmpty(v));
        var item = Entity.Instance.PetList.items.Find(v => v.uniqid == uniqid);
        image.sprite = Resources.Load<Sprite>($"Familiar/{item.Familiar.Image}/base");
        image.enabled = true;
    }
}

