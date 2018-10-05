using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Entities;

public class Unit : MonoBehaviour
{
    public Image character;
    public Image cursor;
    public Slider hp;

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
    }

    public void Setup(PetItem item)
    {
        Setup(item.id);
        character.sprite = Resources.LoadAll<Sprite>($"Familiar/{ new Identify(item.id).Id}/walk")[3];
    }
}
