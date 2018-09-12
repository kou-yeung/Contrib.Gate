using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Home : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClickVending()
    {
        var prefab = Resources.Load("UI/Vending/VendingWindow");
        Instantiate(prefab);
        //Debug.Log("OnClickVending");
    }
}
