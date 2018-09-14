using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entities;

public class Home : MonoBehaviour {

	// Use this for initialization
	IEnumerator Start () {
        yield return Entity.Instance.GetInventory();
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
}

