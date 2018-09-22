using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entities;
using UnityEngine.UI;
using Event;
using UI;

public class Home : MonoBehaviour {

    public Text userName;
    public Text coin;

	// Use this for initialization
	IEnumerator Start () {
        yield return Entity.Instance.GetInventory();
        yield return Entity.Instance.GetEggs();
        yield return Entity.Instance.GetPets();
        Observer.Instance.Subscribe(UserState.Update, UpdateUserState);
        UpdateUserState();
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
        Window.Open<StageWindow>();
    }
    public void OnClickEgg()
    {
        Window.Open<EggWindow>();
    }

    void UpdateUserState(string name = "", object arg = null)
    {
        var state = Entity.Instance.UserState;
        coin.text = state.coin.ToString();
        userName.text = state.playerName;
    }
}

