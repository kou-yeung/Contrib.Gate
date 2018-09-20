using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entities;
using Dungeon;

public class InGame : MonoBehaviour
{
    Stage stage;
    StageInfo stageInfo;
    Entities.Dungeon dungeon;
    Entities.Room room;

    public GameObject[] prefab;    // 一時対応、将来は見た目で変えられるようにします!!
    public GameObject player;       // プレイヤープレハブ

    private void Awake()
    {
        stageInfo = new StageInfo
        {
            dungeonId = new Identify(IDType.Dungeon, 1001),
        };

        room = new Entities.Room
        {
            AreaSize = new Vector2Int(30, 30),
            RoomNum = new Vector2Int(2, 2),
            RoomMin = new Vector2Int(12, 12),
            RoomMax = new Vector2Int(20, 20),
            DeleteRoomTry = 2,
            DeleteRoadTry = 2,
            MergeRoomTry = 1,
        };
    }

    // Use this for initialization
    void Start()
    {
        var add = new Tile[] { Tile.Start, Tile.DownStairs };

        var map = DungeonGen.Gen(0, room.AreaSize, room.RoomNum, room.RoomMin, room.RoomMax, room.DeleteRoadTry, room.DeleteRoadTry, room.MergeRoomTry, add);
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

                if (map[x, y] == Tile.Start)
                {
                    var go = Instantiate(player, this.transform);
                    go.transform.localPosition = new Vector3(x, 0, -y);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    GameObject GetChip(Tile tile)
    {
        switch ((int)tile)
        {
            case (int)Tile.All:
            case (int)Tile.Start:
                return prefab[0];
            case (int)(Tile.UP | Tile.Left):
                return prefab[1];
            case (int)(Tile.UP | Tile.Right):
                return prefab[2];
            case (int)(Tile.UP | Tile.Left | Tile.Right):
                return prefab[3];
            case (int)(Tile.Right | Tile.UP | Tile.Down):
                return prefab[4];
            case (int)(Tile.Left | Tile.UP | Tile.Down):
                return prefab[5];
            case (int)(Tile.Down | Tile.Left):
                return prefab[6];
            case (int)(Tile.Down | Tile.Right):
                return prefab[7];
            case (int)(Tile.Down | Tile.Left | Tile.Right):
                return prefab[8];
            case (int)(Tile.LeftUpCorner):
                return prefab[9];
            case (int)(Tile.RightUpCorner):
                return prefab[10];
            case (int)(Tile.LeftDownCorner):
                return prefab[11];
            case (int)(Tile.RightDownCorner):
                return prefab[12];
            case (int)(Tile.UpStairs):
                return prefab[13];
            case (int)(Tile.DownStairs):
                return prefab[14];
            case (int)(Tile.None):
                return null;
            default:
                {
                    Debug.Log(tile);
                    return null;
                }
        }
    }
}
