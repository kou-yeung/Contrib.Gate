using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Util;
using Event;
using Entities;
using System.Linq;

public class Player : MonoBehaviour
{
    public const string ChangeGridEvent = "Player:ChangeGrid";

    public Action<string> onTriggerEnter;
    public SpriteRenderer sprite;
    public float walkSpeed = 0.1f;
    public float cellSpeed = 5;
    Vector3 move;
    Sprite[] sprites;
    float celloffset;
    new Rigidbody2D rigidbody;
    Vector2 currentPos;
    int cellStartIndex = 0;
    Vector2Int? currentGrid;

    void Start()
    {
        var uniqid = Entity.Instance.StageInfo.pets.Where(v => !string.IsNullOrEmpty(v)).First();
        var pat = Entity.Instance.Pets.Find(uniqid);
        rigidbody = GetComponent<Rigidbody2D>();
        currentPos = rigidbody.position;
        sprites = Resources.LoadAll<Sprite>($"Familiar/{new Identify(pat.id).Id}/walk");
    }

    public void Move(Vector2 move)
    {
        if (move == Vector2.zero) return;
        this.move = new Vector3(move.x, move.y, 0) * walkSpeed;
        if (Mathf.Abs(move.y) >= Mathf.Abs(move.x))
        {
            cellStartIndex = (move.y > 0) ? 9 : 0;
        }
        else
        {
            cellStartIndex = (move.x >= 0) ? 6 : 3;
        }
    }
    // Update is called once per frame
    void Update()
    {
        rigidbody.velocity = (move);

        celloffset += (currentPos - rigidbody.position).sqrMagnitude * cellSpeed;
        currentPos = rigidbody.position;


        sprite.sprite = sprites[cellStartIndex + ((int)celloffset) % 3];
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

        if (currentGrid.HasValue)
        {
            var now = new Vector2Int((int)transform.localPosition.x, (int)transform.localPosition.y);
            if (currentGrid != now)
            {
                currentGrid = now;
                Observer.Instance.Notify(ChangeGridEvent, currentGrid);
            }
        }
        else
        {
            currentGrid = new Vector2Int((int)transform.localPosition.x, (int)transform.localPosition.y);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var e = other.gameObject.GetComponent<MapchipEvent>();
        if (e == null) return;
        onTriggerEnter(EnumExtension<Dungeon.Tile>.ToString(e.tile));
    }

    //void OnTriggerEnter(Collider other)
    //{
    //    if (onTriggerEnter != null)
    //    {
    //        onTriggerEnter();
    //    }
    //}
}
