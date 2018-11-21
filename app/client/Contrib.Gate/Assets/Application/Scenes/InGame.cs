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
using Cinemachine;
using SettlersEngine;

public class InGame : MonoBehaviour
{
    public CinemachineVirtualCamera cinemachineVirtualCamera;
    public Vector2 GridSize = Vector2.one;

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
    PathNode[,] pathnode;

    Player player;
    int EncountRate;
    bool showPeriodMessage; // "そろそろ終わるぞ"メッセージ
    bool blockEvent = false;
    Vector2Int? reserveMove;

    public class PathNode : IPathNode<object>
    {
        static Tile[] walkable = new[] { Tile.All, Tile.Start, Tile.Goal, Tile.UpStairs, Tile.DownStairs };

        public Vector2 pos { get; set; }
        public Tile tile { get; set; }

        public bool IsWalkable(object inContext)
        {
            return walkable.Contains(tile);
        }
    }

    SpatialAStar<PathNode, object> aStar;
    Coroutine playerMove;

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

        pathnode = new PathNode[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                pathnode[x, y] = new PathNode { pos = new Vector2Int(x, y), tile = map[x, y] };

                var prefab = GetChip(map[x, y]);
                if (prefab != null)
                {
                    var go = Instantiate(prefab, this.transform);
                    go.transform.localPosition = new Vector3(x * GridSize.x, 0, -y * GridSize.y);
                    go.name += $"({x},{y})";
                }

                if (map[x, y] == Tile.Start && stageInfo.move == Move.None)
                {
                    var go = Instantiate(playerPrefab, this.transform);
                    go.transform.localPosition = new Vector3(x * GridSize.x, playerPrefab.transform.localPosition.y, -y * GridSize.y);
                    player = go.GetComponent<Player>();
                }

                if ( (map[x, y] == Tile.UpStairs && stageInfo.move == Move.Down) ||
                     (map[x, y] == Tile.DownStairs && stageInfo.move == Move.Up))
                {
                    var offset = new Vector2(x, y);
                    if (x - 1 >= 0 && map[x - 1, y] == Tile.All) offset.x -= 1;
                    else if (x + 1 < width && map[x + 1, y] == Tile.All) offset.x += 1;
                    else if (y - 1 >= 0 && map[y - 1, y] == Tile.All) offset.y -= 1;
                    else if (y + 1 < height && map[y + 1, y] == Tile.All) offset.y += 1;

                    // 上り階段 && 下ってきた場合、あるいは逆の場合プレイヤーを置きます。
                    var go = Instantiate(playerPrefab, this.transform);
                    go.transform.localPosition = new Vector3(offset.x * GridSize.x, playerPrefab.transform.localPosition.y, -offset.y * GridSize.y);
                    player = go.GetComponent<Player>();
                }
            }
        }
        cinemachineVirtualCamera.Follow = player.transform;
        Observer.Instance.Subscribe(MapchipEvent.MoveEvent, OnSubscribe);

        aStar = new SpatialAStar<PathNode, object>(pathnode);
    }

    void Goal()
    {
        var send = new StageEndSend();
        send.stageInfo = Entity.Instance.StageInfo;
        Protocol.Send(send, r =>
        {
            Entity.Instance.StageList.Modify(r.stage);
            Entity.Instance.EggList.Modify(r.eggs);

            Window.Open<BattleResultWindow>(r);
            Observer.Instance.Subscribe(BattleResultWindow.CloseEvent, OnSubscribe);
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
        Observer.Instance.Unsubscribe(MapchipEvent.MoveEvent, OnSubscribe);
    }

    void OnSubscribe(string name, object o)
    {
        switch (name)
        {
            case BattleWindow.CloseEvent:
                player.gameObject.SetActive(true);
                Observer.Instance.Unsubscribe(BattleWindow.CloseEvent, OnSubscribe);
                break;
            case BattleResultWindow.CloseEvent:
                Observer.Instance.Unsubscribe(BattleResultWindow.CloseEvent, OnSubscribe);
                SceneManager.LoadScene(SceneName.Home);
                break;
            case MapchipEvent.MoveEvent:

                if (playerMove == null)
                {
                    var end = (Vector2Int)o;
                    var pos = player.transform.localPosition;
                    var start = Vector2Int.CeilToInt(new Vector2(pos.x / GridSize.x, -pos.z / GridSize.y));
                    var nodes = aStar.Search(start, end, null);
                    playerMove = StartCoroutine(PlayerMove(nodes.ToList()));
                }
                else
                {
                    reserveMove = (Vector2Int)o;
                }
                break;
        }
    }

    /// <summary>
    /// イベント発生したら true を返す。
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    bool OnChangeGrid(PathNode node)
    {
        switch (node.tile)
        {
            case Tile.Goal: Goal(); return true;
            case Tile.UpStairs: Stairs(Move.Up); return true;
            case Tile.DownStairs: Stairs(Move.Down); return true;
            case Tile.All:
                if (UnityEngine.Random.Range(0, 100) < EncountRate)
                {
                    Encount();
                    return true;
                }
                else return false;
            default:
                return false;
        }
    }

    IEnumerator PlayerMove(List<PathNode> nodes)
    {
        var y = player.transform.localPosition.y;

        for (int i = 1; i < nodes.Count; i++)
        {
            var node = nodes[i];

            var from = player.transform.localPosition;
            var to = new Vector3(node.pos.x * GridSize.x, y, -node.pos.y * GridSize.y);
            var time = ((from - to).magnitude / GridSize.x) * .3f;

            var move = LeanTween.moveLocal(player.gameObject, to, time);
            player.Move((nodes[i].pos - nodes[i-1].pos).normalized, time);
            while (LeanTween.isTweening(move.uniqueId)) yield return null;

            // EVENT判定
            if (OnChangeGrid(node)) break;
            if (reserveMove.HasValue)
            {
                StopCoroutine(playerMove);
                playerMove = null;
                var end = reserveMove.Value;
                reserveMove = null;
                Observer.Instance.Notify(MapchipEvent.MoveEvent, end);
                yield break;
            }
        }
        playerMove = null;
    }
    void Update()
    {
        //if (Input.GetKeyUp(KeyCode.Alpha1))
        //{
        //    Encount();
        //}
        //if (Input.GetKeyUp(KeyCode.Alpha1))
        //{
        //    Goal();
        //}

        var remain = Entity.Instance.StageList.period - Util.Time.ServerTime.CurrentUnixTime;
        if(remain <= 0)
        {
            DialogWindow.OpenOk("確認", "ステージが消失しました", () =>
            {
                SceneManager.LoadScene(SceneName.Home);
            });
        }
        else if (!showPeriodMessage && remain < 60*10)
        {
            showPeriodMessage = true;
            DialogWindow.OpenOk("確認", "ステージが消失しそうです");
        }

    }

    void Encount()
    {
        Protocol.Send(new BattleBeginSend { guid = stageInfo.guid }, (r) =>
        {
            Window.Open<BattleWindow>(r);
            Observer.Instance.Subscribe(BattleWindow.CloseEvent, OnSubscribe);
            player.gameObject.SetActive(false);
        }, (error) =>
        {
            // エラー処理
            return false;
        });
    }
    GameObject GetChip(Tile tile)
    {
        // 0 1 2
        // 3 4 5
        // 6 7 8
        switch ((int)tile)
        {
            // 0 1 2 
            case (int)(Tile.UP | Tile.Left):
                return prefab[0];
            case (int)(Tile.UP | Tile.Left | Tile.Right):
                return prefab[1];
            case (int)(Tile.UP | Tile.Right):
                return prefab[2];

            // 3 4 5 
            case (int)(Tile.Left | Tile.UP | Tile.Down):
                return prefab[3];
            case (int)(Tile.All):
                return prefab[4];
            case (int)(Tile.Right | Tile.UP | Tile.Down):
                return prefab[5];

            // 6 7 8 
            case (int)(Tile.Down | Tile.Left):
                return prefab[6];
            case (int)(Tile.Down | Tile.Left | Tile.Right):
                return prefab[7];
            case (int)(Tile.Down | Tile.Right):
                return prefab[8];

            // 角
            case (int)(Tile.LeftUpCorner):
                return prefab[9];
            case (int)(Tile.RightUpCorner):
                return prefab[10];
            case (int)(Tile.LeftDownCorner):
                return prefab[11];
            case (int)(Tile.RightDownCorner):
                return prefab[12];

            // 階段
            case (int)(Tile.UpStairs):
                return prefab[13];
            case (int)(Tile.DownStairs):
                return prefab[14];

            // 開始
            case (int)(Tile.Start):
                return prefab[15];

            case (int)(Tile.Goal):
                return prefab[16];
            default:
                {
                    Debug.Log(tile);
                    return null;
                }
        }
    }
}
