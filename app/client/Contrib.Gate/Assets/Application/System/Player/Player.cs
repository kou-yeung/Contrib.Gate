using UnityEngine;
using Entities;

public class Player : MonoBehaviour
{
    public SpriteRenderer sprite;
    Sprite[] sprites;
    int cellStartIndex = 0;

    public void Setup(string uniqid)
    {
        var item = Entity.Instance.PetList.Find(uniqid);
        sprites = Resources.LoadAll<Sprite>($"Familiar/{item.Familiar.Image}/walk");
        sprite.sprite = sprites[cellStartIndex + 1];
    }

    public void Move(Vector2 dir, float time)
    {
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

