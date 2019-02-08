using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Entities;
using System;
using Battle;
using EventSystem;

public class Unit : MonoBehaviour
{
    public enum Side
    {
        Enemy,
        Player,
    }

    public Image character;
    public CircleOutline outline;
    public Slider hp;
    public Side side { get; private set; }

    Identify id;

    object item;
    public EnemyItem EnemyItem { get { return item as EnemyItem; } }
    public PetItem PetItem { get { return item as PetItem; } }
    public Params Params { get; private set; }
    public Race Race { get; private set; }
    public int[] Attribute { get; private set; }

    public int MaxHP { get; private set; }
    public int MaxMP { get; private set; }
    public int Level { get; private set; }
    public bool IsDead { get { return Params[Param.HP] <= 0; } }

    int? outlineId;
    private void Start()
    {
        outlineId = LeanTween.value(0f, 1f, 0.5f).setLoopPingPong().setOnUpdate( (float v) =>
        {
            var color = outline.EffectColor;
            color.a = v;
            outline.EffectColor = color;
        }).id;
    }
    public void Setup(Identify id)
    {
        this.id = id;
        outline.gameObject.SetActive(false);
    }

    public void Setup(EnemyItem item)
    {
        this.item = item;
        this.Params = new Params(item);
        MaxHP = this.Params[Param.HP];
        MaxMP = this.Params[Param.MP];
        this.Race = item.Enemy.Race;
        this.Attribute = new int[] { 0, 0, 0, 0 };
        this.Level = item.Level;

        Setup(item.id);
        character.sprite = Resources.Load<Sprite>($"Familiar/{ item.Enemy.Image}/base");
        side = Side.Enemy;
    }

    public void Setup(PetItem item)
    {
        this.item = item;
        this.Params = new Params(item);
        MaxHP = this.Params[Param.HP];
        MaxMP = this.Params[Param.MP];
        this.Race = item.Familiar.Race;
        this.Attribute = item.Familiar.Attribute;
        this.Level = item.level;

        Setup(item.id);
        character.sprite = Resources.LoadAll<Sprite>($"Familiar/{item.Familiar.Image}/face")[0];
        side = Side.Player;
    }

    public void Focus(Action cb = null)
    {
        LeanTween.moveLocalY(character.gameObject, 15, 0.1f).setLoopPingPong(1).setOnComplete(cb);
    }
    private void OnDestroy()
    {
        if(outlineId.HasValue) LeanTween.cancel(outlineId.Value);
    }

    /// <summary>
    /// ダメージを受けた
    /// </summary>
    /// <param name="damage"></param>
    public void Damage(int damage)
    {
        Params[Param.HP] -= damage;
        hp.value = (float)Params[Param.HP] / (float)MaxHP;

        if (IsDead) this.gameObject.SetActive(false);   /// TODO : 演出追加!!現在はとりあえず消す
    }

    public bool Selectable
    {
        get
        {
            return outline.gameObject.activeSelf;
        }
        set
        {
            outline.gameObject.SetActive(value);
            GetComponent<Button>().interactable = value;
        }
    }
}
