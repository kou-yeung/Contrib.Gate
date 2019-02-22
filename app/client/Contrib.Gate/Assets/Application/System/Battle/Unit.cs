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
    public Text hpText;
    public Text level;
    public Text damageText;
    public Side side { get; private set; }

    Identify id;

    object item;
    public EnemyItem EnemyItem { get { return item as EnemyItem; } }
    public PetItem PetItem { get { return item as PetItem; } }
    public Params Params { get; private set; }
    public Race Race { get; private set; }
    public Attributes Attributes { get; private set; }

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
        this.Race = item.Familiar.Race;
        this.Attributes = new Attributes(item.Familiar.Attribute);
        this.Level = item.Level;

        level.text = $"Lv.{Level}";
        hpText.text = $"{Params[Param.HP]}/{MaxHP}";

        Setup(item.id);
        character.sprite = Resources.Load<Sprite>($"Familiar/{ item.Familiar.Image}/base");
        side = Side.Enemy;
    }

    public void Setup(PetItem item)
    {
        this.item = item;
        this.Params = new Params(item);
        MaxHP = this.Params[Param.HP];
        MaxMP = this.Params[Param.MP];
        this.Race = item.Familiar.Race;
        this.Attributes = new Attributes(item.Familiar.Attribute);
        this.Level = item.level;

        level.text = $"Lv.{Level}";
        hpText.text = $"{Params[Param.HP]}/{MaxHP}";

        Setup(item.id);
        character.sprite = Resources.LoadAll<Sprite>($"Familiar/{item.Familiar.Image}/face")[0];
        side = Side.Player;
    }

    public void Focus(Action cb = null)
    {
        LeanTween.moveLocalY(character.gameObject, 15, 0.1f).setLoopPingPong(1).setOnComplete(cb);
    }

    public void Shake(Action cb = null)
    {
        var rand = (UnityEngine.Random.Range(0, 100) < 50) ? 1 : -1;
        LeanTween.moveLocal(character.gameObject, new Vector3(15 * rand, 0, 0), 0.4f).setEasePunch().setOnComplete(cb);
    }

    private void OnDestroy()
    {
        if(outlineId.HasValue) LeanTween.cancel(outlineId.Value);
    }

    /// <summary>
    /// ダメージを受けた
    /// </summary>
    /// <param name="damage"></param>
    public void Damage(int damage, Action cb)
    {
        if (damage <= 0)
        {
            // ミスと表示
            ShowDamageText("ミス", () =>
            {
                cb?.Invoke();
            });
        }
        else
        {
            Shake();

            Params[Param.HP] = Mathf.Max(0, Params[Param.HP] - damage);
            hp.value = (float)Params[Param.HP] / (float)MaxHP;
            hpText.text = $"{Params[Param.HP]}/{MaxHP}";

            ShowDamageText(damage.ToString(), () =>
            {
                if (IsDead)
                {
                    var group = GetComponent<CanvasGroup>();
                    LeanTween.value(1.0f, 0.0f, 0.5f).setOnUpdate((float v) =>
                    {
                        group.alpha = v;
                    }).setOnComplete(() =>
                    {
                        this.gameObject.SetActive(false);   /// TODO : 演出追加!!現在はとりあえず消す
                        cb?.Invoke();
                    });
                }
                else
                {
                    cb?.Invoke();
                }
            });
        }
    }

    void ShowDamageText(string text, Action cb)
    {
        // ダメージテキスト表示
        damageText.gameObject.SetActive(true);
        damageText.text = text;
        damageText.gameObject.transform.localScale = Vector3.one;
        LeanTween.scale(damageText.gameObject, Vector3.one * 1.5f, 0.5f).setOnComplete(() =>
        {
            damageText.gameObject.SetActive(false);
            cb?.Invoke();
        });
        LeanTween.value(0.3f, 1.0f, 0.5f).setOnUpdate((float v) =>
        {
            var color = damageText.color;
            color.a = v;
            damageText.color = color;
        }).setLoopPingPong(1);
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
