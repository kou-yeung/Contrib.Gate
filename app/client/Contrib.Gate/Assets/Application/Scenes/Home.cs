using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entities;
using UnityEngine.UI;
using Event;

public class Home : MonoBehaviour {

    public Text userName;
    public Text coin;

	// Use this for initialization
	IEnumerator Start () {
        yield return Entity.Instance.GetInventory();
        Observer.Instance.Subscribe(UserState.Update, UpdateUserState);
        UpdateUserState();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClickVending()
    {
        Instantiate(Resources.Load("UI/Vending/VendingWindow"));
    }

    public void OnClickBake()
    {
        Instantiate(Resources.Load("UI/Bake/BakeWindow"));
    }

    public void OnClickDebug()
    {
        Instantiate(Resources.Load("UI/Debug/DebugWindow"));
    }
    public void OnClickInventory()
    {
        Instantiate(Resources.Load("UI/Inventory/InventoryWindow"));
    }

    void UpdateUserState(string name = "", object arg = null)
    {
        var state = Entity.Instance.UserState;
        coin.text = state.coin.ToString();
        userName.text = state.playerName;
    }
}

