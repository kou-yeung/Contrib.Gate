using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    public Action onTriggerEnter;
    public SpriteRenderer sprite;
    public float walkSpeed = 0.1f;
    public float cellSpeed = 5;
    Vector3 move;
    Sprite[] sprites;
    float celloffset;

    void Start()
    {
        sprites = Resources.LoadAll<Sprite>($"Familiar/{1001}/walk");
    }

    public void Move(Vector2 move)
    {
        this.move = new Vector3(move.x, 0, move.y) * walkSpeed;
    }
    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKey(KeyCode.W)) move.z = Mathf.Clamp(move.z + .05f, -walkSpeed, walkSpeed);
        //if (Input.GetKey(KeyCode.S)) move.z = Mathf.Clamp(move.z - .05f, -walkSpeed, walkSpeed);
        //if (Input.GetKey(KeyCode.A)) move.x = Mathf.Clamp(move.x - .05f, -walkSpeed, walkSpeed);
        //if (Input.GetKey(KeyCode.D)) move.x = Mathf.Clamp(move.x + .05f, -walkSpeed, walkSpeed);

        this.transform.localPosition += move;

        celloffset += move.sqrMagnitude * cellSpeed;

        var startIndex = 0;
        if (Mathf.Abs(move.z) >= Mathf.Abs(move.x))
        {
            startIndex = (move.z > 0) ? 9 : 0;
        }
        else
        {
            startIndex = (move.x >= 0) ? 6 : 3;
        }

        //Debug.Log(string.Format("({0}) {1}",move.ToString(), startIndex));
        sprite.sprite = sprites[startIndex + ((int)celloffset) % 3];

        move *= 0.75f;
        if (Mathf.Abs(move.x) < 0.01) move.x = 0;
        if (Mathf.Abs(move.z) < 0.01) move.z = 0;

        // カメラはプレイヤーに
        Camera.main.transform.position = transform.localPosition + Vector3.up;
        //// プレイやはカメラに
        //Vector3 p = Camera.main.transform.position;
        //p.y = transform.position.y;
        //transform.LookAt(p);
    }

    void OnTriggerEnter(Collider other)
    {
        if (onTriggerEnter != null)
        {
            onTriggerEnter();
        }
    }
}
