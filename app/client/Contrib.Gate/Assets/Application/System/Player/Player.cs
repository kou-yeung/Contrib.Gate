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
    new Rigidbody rigidbody;
    Vector2 currentPos;
    int cellStartIndex = 0;
    Vector2Int? currentGrid;

    void Start()
    {
        var uniqid = Entity.Instance.StageInfo.pets.Where(v => !string.IsNullOrEmpty(v)).First();
        var pat = Entity.Instance.PetList.Find(uniqid);
        rigidbody = GetComponent<Rigidbody>();
        currentPos = rigidbody.position;
        sprites = Resources.LoadAll<Sprite>($"Familiar/{pat.Familiar.Image}/walk");
    }

    public void Move(Vector2 move)
    {
        if (move == Vector2.zero) return;
        move = move.Rotate(45);
        this.move = new Vector3(move.x, 0, move.y) * walkSpeed;


        var normalized = new Vector2(this.move.x, this.move.z).Rotate(-45).normalized;
        var rot = (Mathf.Atan2(normalized.x, normalized.y) * Mathf.Rad2Deg) + 180;

        if (rot >= 75 && rot <= 105)
        {
            // 左
            cellStartIndex = 3;
        }
        else if (rot >= 105 && rot <= 255)
        {
            // 上
            cellStartIndex = 9;
        }
        else if (rot >= 255 && rot <= 285)
        {
            // 右
            cellStartIndex = 6;
        }
        else
        {
            // 下
            cellStartIndex = 0;
        }
    }
    // Update is called once per frame
    void Update()
    {
        rigidbody.velocity = (move);

        celloffset += rigidbody.velocity.magnitude * cellSpeed;
        currentPos = rigidbody.position;


        sprite.sprite = sprites[cellStartIndex + ((int)celloffset) % 3];
        move *= 0.75f;
        if (Mathf.Abs(move.x) < 0.01) move.x = 0;
        if (Mathf.Abs(move.z) < 0.01) move.z = 0;

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

    void OnTriggerEnter(Collider other)
    {
        var e = other.gameObject.GetComponent<MapchipEvent>();
        if (e == null) return;
        onTriggerEnter(EnumExtension<Dungeon.Tile>.ToString(e.tile));
    }
}

