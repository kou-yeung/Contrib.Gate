using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Entities;

public class Unit : MonoBehaviour
{
    public enum Side
    {
        Enemy,
        Player,
    }

    public Image character;
    public Image cursor;
    public Slider hp;
    public Side side { get; private set; }
    Identify id;

    public void Setup(Identify id)
    {
        this.id = id;
        cursor.gameObject.SetActive(false);
    }

    public void Setup(EnemyItem item)
    {
        Setup(item.id);
        character.sprite = Resources.LoadAll<Sprite>($"Familiar/{ new Identify(item.id).Id}/walk")[7];
        side = Side.Enemy;
    }

    public void Setup(PetItem item)
    {
        Setup(item.id);
        character.sprite = Resources.LoadAll<Sprite>($"Familiar/{ new Identify(item.id).Id}/walk")[4];
        side = Side.Player;
    }

    public void Focus()
    {
        if (side == Side.Player)
        {
            LeanTween.moveLocalY(character.gameObject, 15, 0.1f).setLoopPingPong(1);
            LeanTween.value(0f, 3f, 0.15f).setOnUpdate((float v) =>
            {
                var index = new[] { 1, 7, 10, 4 };
                var i = index[(int)v];
                character.sprite = Resources.LoadAll<Sprite>($"Familiar/{ new Identify(id).Id}/walk")[i];
            });
        }
    }
}
