using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dungeon;
using UnityEngine.UI;

namespace Test
{
    public class dungeon : MonoBehaviour
    {
        public GameObject[] chip;

        void Start()
        {
            var additional = new[] { Tile.UpStairs, Tile.DownStairs };
            var map = DungeonGen.Gen(0, new Vector2Int(30, 30), new Vector2Int(3, 3), new Vector2Int(12, 12), new Vector2Int(20, 20), 2, 2, 1, additional);

            var width = map.GetLength(0);
            var height = map.GetLength(1);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var prefab = GetChip(map[x, y]);
                    if (prefab != null)
                    {
                        var go = Instantiate(prefab, this.transform);
                        go.transform.localPosition = new Vector3(x, 0, -y);
                    }
                }
            }
        }
        GameObject GetChip(Tile tile)
        {
            switch ((int)tile)
            {
                case (int)Tile.All:
                    return chip[0];
                case (int)(Tile.UP | Tile.Left):
                    return chip[1];
                case (int)(Tile.UP | Tile.Right):
                    return chip[2];
                case (int)(Tile.UP | Tile.Left | Tile.Right):
                    return chip[3];
                case (int)(Tile.Right | Tile.UP | Tile.Down):
                    return chip[4];
                case (int)(Tile.Left | Tile.UP | Tile.Down):
                    return chip[5];
                case (int)(Tile.Down | Tile.Left):
                    return chip[6];
                case (int)(Tile.Down | Tile.Right):
                    return chip[7];
                case (int)(Tile.Down | Tile.Left | Tile.Right):
                    return chip[8];
                case (int)(Tile.LeftUpCorner):
                    return chip[9];
                case (int)(Tile.RightUpCorner):
                    return chip[10];
                case (int)(Tile.LeftDownCorner):
                    return chip[11];
                case (int)(Tile.RightDownCorner):
                    return chip[12];
                case (int)(Tile.UpStairs):
                    return chip[13];
                case (int)(Tile.DownStairs):
                    return chip[14];
                case (int)(Tile.None):
                    return null;
                default:
                    {
                        Debug.Log(tile);
                        return null;
                    }
            }
        }

        void Update()
        {

        }
    }
}
