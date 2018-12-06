using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dungeon;
using UnityEngine.EventSystems;
using EventSystem;
using System.Text.RegularExpressions;

[RequireComponent(typeof(EventTrigger))]
public class MapchipEvent : MonoBehaviour
{
    public const string MoveEvent = @"MapchipEvent:Move";
    public Tile tile;

    public void Move()
    {
        var m = Regex.Match(name, @"([-\d]+),([-\d]+)");
        if (m == Match.Empty) return;

        var x = int.Parse(m.Groups[1].ToString());
        var y = int.Parse(m.Groups[2].ToString());
        Observer.Instance.Notify(MoveEvent, new Vector2Int(x, y));
    }
}
