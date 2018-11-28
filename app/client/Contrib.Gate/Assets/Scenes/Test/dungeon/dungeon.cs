using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dungeon;
using UnityEngine.UI;

namespace Test
{
    public class dungeon : MonoBehaviour
    {
        Vector2 GridSize = Vector2.one;

        void Start()
        {
            var mapchip = Resources.Load<MapchipSet>("Dungeon/001");
            GridSize = mapchip.GridSize;
            var additional = new[] { Tile.UpStairs, Tile.DownStairs };
            var map = DungeonGen.Gen(0, new Vector2Int(30, 30), new Vector2Int(3, 3), new Vector2Int(12, 12), new Vector2Int(20, 20), 2, 2, 1, additional);

            var width = map.GetLength(0);
            var height = map.GetLength(1);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var prefab = mapchip.GetChip(map[x, y]);
                    if (prefab != null)
                    {
                        var go = Instantiate(prefab, this.transform);
                        go.transform.localPosition = new Vector3(x * GridSize.x, -8, -y * GridSize.y);
                    }
                }
            }
        }
    }
}
