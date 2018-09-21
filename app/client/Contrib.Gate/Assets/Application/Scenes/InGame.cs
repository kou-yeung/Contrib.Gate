using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entities;
using Dungeon;
using Network;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

public class InGame : MonoBehaviour
{
    //Stage stage;

    StageInfo stageInfo
    {
        get { return Entity.Instance.StageInfo; }
    }

    Entities.Dungeon dungeon
    {
        get
        {
            return Array.Find(Entity.Instance.Dungeons, v => v.Identify == stageInfo.dungeonId);
        }
    }
    Entities.Room room
    {
        get
        {
            var dungeon = this.dungeon;
            return Array.Find(Entity.Instance.Rooms, v => v.Identify == dungeon.Room);
        }
    }

    public GameObject[] prefab;    // 一時対応、将来は見た目で変えられるようにします!!
    public GameObject player;       // プレイヤープレハブ


    void Start()
    {
        var tiles = new List<Tile>();// new Tile[] { Tile.Start, Tile.DownStairs };
        var dungeon = this.dungeon;
        // 上り
        if (dungeon.UpFloor == new Identify(IDType.Dungeon, 0))
        {
            tiles.Add(Tile.Start);  // なければ開始マスを決める
        }
        else
        {
            tiles.Add(Tile.UpStairs);
        }

        // 下り
        if (dungeon.DownFloor == new Identify(IDType.Dungeon, 999999))
        {
            tiles.Add(Tile.Goal);  // なければゴールマスを決める
        }
        else
        {
            tiles.Add(Tile.DownStairs);
        }

        var map = DungeonGen.Gen(0, room.AreaSize, room.RoomNum, room.RoomMin, room.RoomMax, room.DeleteRoadTry, room.DeleteRoadTry, room.MergeRoomTry, tiles.ToArray());
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

                if (map[x, y] == Tile.Start && stageInfo.move == Move.None)
                {
                    var go = Instantiate(player, this.transform);
                    go.transform.localPosition = new Vector3(x, 0, -y);
                }

                if ( (map[x, y] == Tile.UpStairs && stageInfo.move == Move.Down) ||
                     (map[x, y] == Tile.DownStairs && stageInfo.move == Move.Up))
                {
                    // 上り階段 && 下ってきた場合、あるいは逆の場合プレイヤーを置きます。
                    var go = Instantiate(player, this.transform);
                    go.transform.localPosition = new Vector3(x, 0, -y);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            if (dungeon.UpFloor != new Identify(IDType.Dungeon, 0))
            {
                var send = new StageMoveSend();
                send.stageInfo = stageInfo;
                send.stageInfo.move = Move.Up;
                Protocol.Send(send, (r) =>
                {
                    Entity.Instance.UpdateStageInfo(r.stageInfo);
                    SceneManager.LoadScene(SceneName.InGame);
                });
            }
        }

        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            if (dungeon.DownFloor != new Identify(IDType.Dungeon, 999999))
            {
                var send = new StageMoveSend();
                send.stageInfo = stageInfo;
                send.stageInfo.move = Move.Down;
                Protocol.Send(send, (r) =>
                {
                    Entity.Instance.UpdateStageInfo(r.stageInfo);
                    SceneManager.LoadScene(SceneName.InGame);
                });
            }
        }
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
