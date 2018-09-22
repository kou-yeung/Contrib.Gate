using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    public Action onTriggerEnter;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var pos = this.transform.localPosition;

        if (Input.GetKey(KeyCode.W)) pos.z -= .1f;
        if (Input.GetKey(KeyCode.S)) pos.z += .1f;
        if (Input.GetKey(KeyCode.A)) pos.x += .1f;
        if (Input.GetKey(KeyCode.D)) pos.x -= .1f;

        Camera.main.transform.LookAt(this.transform);
        this.transform.localPosition = pos;

    }

    void OnTriggerEnter(Collider other)
    {
        if (onTriggerEnter != null)
        {
            onTriggerEnter();
        }
    }
}
