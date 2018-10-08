using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entities;
using Dungeon;
using Network;
using UnityEngine.SceneManagement;
using System.Linq;
using System;
using UI;
using Util;
using Event;

public class InGame : MonoBehaviour
{
    public Joystick joystick;

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
    public GameObject playerPrefab;      // プレイヤープレハブ

    Tile[,] map;
    Player player;
    int EncountRate;

    void Start()
    {
        var tiles = new List<Tile>();
        var dungeon = this.dungeon;
        EncountRate = this.dungeon.EncountRate;
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

        map = DungeonGen.Gen(stageInfo.seed, room.AreaSize, room.RoomNum, room.RoomMin, room.RoomMax, room.DeleteRoadTry, room.DeleteRoadTry, room.MergeRoomTry, tiles.ToArray());
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
                    go.transform.localPosition = new Vector3(x, y, 0);
                }

                if (map[x, y] == Tile.Start && stageInfo.move == Move.None)
                {
                    var go = Instantiate(playerPrefab, this.transform);
                    go.transform.localPosition = new Vector3(x, y, 0);
                    player = go.GetComponent<Player>();
                    player.onTriggerEnter = onTriggerEnter;
                }

                if ( (map[x, y] == Tile.UpStairs && stageInfo.move == Move.Down) ||
                     (map[x, y] == Tile.DownStairs && stageInfo.move == Move.Up))
                {
                    var offset = new Vector3(0, 0, 0);
                    if (x - 1 >= 0 && map[x - 1, y] == Tile.All) offset.x -= 1;
                    else if (x + 1 < width && map[x + 1, y] == Tile.All) offset.x += 1;
                    else if (y - 1 >= 0 && map[y - 1, y] == Tile.All) offset.y -= 1;
                    else if (y + 1 < height && map[y + 1, y] == Tile.All) offset.y += 1;

                    // 上り階段 && 下ってきた場合、あるいは逆の場合プレイヤーを置きます。
                    var go = Instantiate(playerPrefab, this.transform);
                    go.transform.localPosition = new Vector3(x, y, 0) + offset;
                    player = go.GetComponent<Player>();
                    player.onTriggerEnter = onTriggerEnter;
                }
            }
        }

        Observer.Instance.Subscribe(Player.ChangeGridEvent, OnSubscribe);
    }

    bool flag = false;

    void onTriggerEnter(string s)
    {
        if (flag) return;
        flag = true;

        switch (EnumExtension<Tile>.Parse(s))
        {
            case Tile.Goal: Goal(); break;
            case Tile.UpStairs: Stairs(Move.Up); break;
            case Tile.DownStairs: Stairs(Move.Down); break;
        }
    }
    void Goal()
    {
        var send = new StageEndSend();
        send.stageInfo = Entity.Instance.StageInfo;
        Protocol.Send(send, r =>
        {
            Entity.Instance.StageList.Modify(r.stage);
            DialogWindow.OpenOk("おめでとう", $"ステージクリアしました", () =>
            {
                SceneManager.LoadScene(SceneName.Home);
            });
        });
    }

    /// <summary>
    /// 階段移動
    /// </summary>
    /// <param name="move"></param>
    void Stairs(Move move)
    {
        var send = new StageMoveSend();
        send.stageInfo = stageInfo;
        send.stageInfo.move = move;
        Protocol.Send(send, (r) =>
        {
            Entity.Instance.UpdateStageInfo(r.stageInfo);
            SceneManager.LoadScene(SceneName.InGame);
        });
    }

    private void OnDestroy()
    {
        Observer.Instance.Unsubscribe(Player.ChangeGridEvent, OnSubscribe);
    }

    void OnSubscribe(string name, object o)
    {
        switch (name)
        {
            case BattleWindow.CloseEvent:
                joystick.enabled = true;
                Observer.Instance.Unsubscribe(BattleWindow.CloseEvent, OnSubscribe);
                break;
            case Player.ChangeGridEvent:

                var grid = (Vector2Int)o;
                if(map[grid.x, grid.y] == Tile.All)
                {
                    if (UnityEngine.Random.Range(0, 100) < EncountRate)
                    {
                        Encount();
                    }
                }
                break;
        }
    }
    void Update()
    {
        if (!joystick.enabled) return;

        player.Move(joystick.Position);
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            Encount();
        }

        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            Goal();
        }
    }

    void Encount()
    {
        joystick.enabled = false;
        Protocol.Send(new BattleBeginSend { guid = stageInfo.guid }, (r) =>
        {
            Window.Open<BattleWindow>(r);
            Observer.Instance.Subscribe(BattleWindow.CloseEvent, OnSubscribe);
        }, (error) =>
        {
            // エラー処理
            joystick.enabled = true;
            return false;
        });
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
            case (int)(Tile.Goal):
                return prefab[15];
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
