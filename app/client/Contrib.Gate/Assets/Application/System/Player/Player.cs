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
    //Rigidbody2D
    void Start()
    {
        sprites = Resources.LoadAll<Sprite>($"Familiar/{1001}/walk");
    }

    public void Move(Vector2 move)
    {
        this.move = new Vector3(move.x, move.y, 0) * walkSpeed;
    }
    // Update is called once per frame
    void Update()
    {
        this.transform.localPosition += move;

        celloffset += move.sqrMagnitude * cellSpeed;

        var startIndex = 0;
        if (Mathf.Abs(move.y) >= Mathf.Abs(move.x))
        {
            startIndex = (move.y > 0) ? 9 : 0;
        }
        else
        {
            startIndex = (move.x >= 0) ? 6 : 3;
        }

        sprite.sprite = sprites[startIndex + ((int)celloffset) % 3];

        move *= 0.75f;
        if (Mathf.Abs(move.x) < 0.01) move.x = 0;
        if (Mathf.Abs(move.y) < 0.01) move.y = 0;

        // カメラはプレイヤーに
        var pos = Camera.main.transform.position;
        pos.x = transform.localPosition.x;
        pos.y = transform.localPosition.y;
        Camera.main.transform.position = pos;
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
