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
    public SpriteRenderer sprite;
    public float walkSpeed = 0.1f;
    public float cellSpeed = 5;
    Vector3 move;
    Sprite[] sprites;
    float celloffset;
    new Rigidbody rigidbody;
    Vector2 currentPos;
    int cellStartIndex = 0;

    void Start()
    {
        var uniqid = Entity.Instance.StageInfo.pets.Where(v => !string.IsNullOrEmpty(v)).First();
        var pat = Entity.Instance.PetList.Find(uniqid);
        rigidbody = GetComponent<Rigidbody>();
        currentPos = rigidbody.position;
        sprites = Resources.LoadAll<Sprite>($"Familiar/{pat.Familiar.Image}/walk");
        sprite.sprite = sprites[cellStartIndex + 1];
    }

    public void Move(Vector2 dir, float time)
    {
        //Debug.Log(Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
        dir = dir.Rotate(-45);
        if (dir.x == 0)
        {
            cellStartIndex = (dir.y < 0) ? 6 : 3;
        } else
        {
            cellStartIndex = (dir.x < 0) ? 9 : 0;
        }

        LeanTween.value(1, 4, time).setOnUpdate((float v) =>
        {
            sprite.sprite = sprites[cellStartIndex + ((int)v) % 3];
        });
    }
}

