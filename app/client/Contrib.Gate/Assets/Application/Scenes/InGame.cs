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
using EventSystem;
using Cinemachine;
using SettlersEngine;
using UnityEngine.UI;
using UnityEngine.PostProcessing;

public class InGame : MonoBehaviour
{
    /// <summary>
    /// 動作
    /// </summary>
    enum State
    {
        Move,
        Event,
    }

    public CinemachineVirtualCamera cinemachineVirtualCamera;
    Vector2 GridSize = Vector2.one;

    StageInfo stageInfo
    {
        get { return Entity.Instance.StageInfo; }
    }

    Entities.Dungeon dungeon;
    Entities.Dungeon Dungeon
    {
        get
        {
            if (dungeon == null)
            {
                dungeon = Array.Find(Entity.Instance.Dungeons, v => v.Identify == stageInfo.dungeonId);
            }
            return dungeon;
        }
    }
    Entities.Room room;
    Entities.Room Room
    {
        get
        {
            if (room == null)
            {
                room = Array.Find(Entity.Instance.Rooms, v => v.Identify == Dungeon.Room);
            }
            return room;
        }
    }
    int EncountRate { get { return Dungeon.EncountRate; } }

    public GameObject playerPrefab;     // プレイヤープレハブ
    public Text periodMessage;
    public Transform dungeonRoot;
    List<Player> players = new List<Player>();
    Tile[,] map;
    Vector2Int? reserveMove;

    AStar aStar;
    Coroutine playerMove;
    State stage;

    void Start()
    {
        var mapchip = Resources.Load<MapchipSet>($"Dungeon/{Dungeon.AssetPath}/MapchipSet");
        GridSize = mapchip.GridSize;
        var postProcessing = Camera.main.GetComponent<PostProcessingBehaviour>();
        if (postProcessing != null) postProcessing.profile = mapchip.PostProcessingProfile;

        map = DungeonGen.Gen(stageInfo.seed, Room.AreaSize, Room.RoomNum, Room.RoomMin, Room.RoomMax, Room.DeleteRoadTry, Room.DeleteRoadTry, Room.MergeRoomTry, GetAdditionalTile(Dungeon));
        var width = map.GetLength(0);
        var height = map.GetLength(1);

        Vector2Int? playerGrid = null;
        var random = new System.Random(stageInfo.seed);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var info = mapchip.GetChip(map[x, y], random);
                if (info != null)
                {
                    var go = Instantiate(info.prefab, Vector3.zero, info.Quaternion, dungeonRoot);
                    go.transform.localRotation = info.Quaternion;
                    go.transform.localPosition = Grid2WorldPosition(new Vector2Int(x, y), GridSize, info.prefab.transform.localPosition);
                    go.name += $"({x},{y})";
                }

                if (map[x, y] == Tile.Start && stageInfo.move == Move.None)
                {
                    playerGrid = new Vector2Int(x, y);
                }
                if ( (map[x, y] == Tile.UpStairs && stageInfo.move == Move.Down) ||
                     (map[x, y] == Tile.DownStairs && stageInfo.move == Move.Up))
                {
                    var pos = new Vector2Int(x, y);
                    if (x - 1 >= 0 && map[x - 1, y] == Tile.All) pos.x -= 1;
                    else if (x + 1 < width && map[x + 1, y] == Tile.All) pos.x += 1;
                    else if (y - 1 >= 0 && map[y - 1, y] == Tile.All) pos.y -= 1;
                    else if (y + 1 < height && map[y + 1, y] == Tile.All) pos.y += 1;
                    playerGrid = pos;
                }
            }
        }

        if (playerGrid.HasValue)
        {
            var position = Grid2WorldPosition(playerGrid.Value, GridSize, new Vector3(0, playerPrefab.transform.localPosition.y, 0));

            foreach (var uniq in Entity.Instance.StageInfo.pets)
            {
                var go = Instantiate(playerPrefab, dungeonRoot);
                go.transform.localPosition = position;
                var player = go.GetComponent<Player>();
                player.Setup(uniq);
                players.Add(player);
            }
        }

        cinemachineVirtualCamera.Follow = players.First().transform;
        Observer.Instance.Subscribe(MapchipEvent.MoveEvent, OnSubscribe);

        // 経路探索用 A* 生成
        aStar = new AStar(map);
    }

    /// <summary>
    /// ダンジョン情報から特別のタイル一覧を生成する
    /// </summary>
    /// <param name="dungeon"></param>
    /// <returns></returns>
    Tile[] GetAdditionalTile(Entities.Dungeon dungeon)
    {
        // 上り
        var tiles = new List<Tile>();
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
        return tiles.ToArray();
    }

    /// <summary>
    /// ゴールにたどり着いた
    /// </summary>
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
                foreach (var player in players)
                {
                    player.gameObject.SetActive(true);
                }
                stage = State.Move;
                Observer.Instance.Unsubscribe(BattleWindow.CloseEvent, OnSubscribe);
                ChromaticAberration(1f, 0f, 0.1f);
                break;
            case BattleResultWindow.CloseEvent:
                Observer.Instance.Unsubscribe(BattleResultWindow.CloseEvent, OnSubscribe);
                SceneManager.LoadScene(SceneName.Home);
                break;
            case MapchipEvent.MoveEvent:
                if (stage != State.Move) return;
                if (playerMove == null)
                {
                    var end = (Vector2Int)o;
                    var pos = players.First().transform.localPosition;
                    var start = Vector2Int.CeilToInt(new Vector2(pos.x / GridSize.x, -pos.z / GridSize.y));
                    playerMove = StartCoroutine(PlayerMove(aStar.Search(start, end)));
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
    bool OnEventEvolution(PathNode node)
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
        var first = players.First();
        var offset = new Vector3(0, first.transform.localPosition.y,0);

        for (int i = 1; i < nodes.Count; i++)
        {
            var node = nodes[i];

            var from = first.transform.localPosition;
            var to = Grid2WorldPosition(Vector2Int.CeilToInt(node.pos), GridSize, offset);
            var time = ((from - to).normalized.magnitude) * .2f;

            var move = LeanTween.moveLocal(first.gameObject, to, time);
            first.Move((nodes[i].pos - nodes[i-1].pos).normalized, time);

            // 追尾するキャラの移動
            for (int j = 1; j < players.Count; j++)
            {
                from = players[j].transform.localPosition;
                to = players[j - 1].transform.localPosition;
                time = ((from - to).normalized.magnitude) * .2f;

                LeanTween.moveLocal(players[j].gameObject, to, time);
                players[j].Move(((Vector2)(WorldPosition2Grid(to, GridSize) - WorldPosition2Grid(from, GridSize))).normalized, time);
            }

            while (LeanTween.isTweening(move.uniqueId)) yield return null;

            // EVENT判定
            if (OnEventEvolution(node))
            {
                stage = State.Event;
                break;
            }
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
        if (stage != State.Move) return;
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
            stage = State.Event;
            DialogWindow.OpenOk("確認", "ステージが消失しました", () =>
            {
                SceneManager.LoadScene(SceneName.Home);
            });
        }
        else if (!periodMessage.gameObject.activeSelf && remain < 60*10)
        {
            periodMessage.gameObject.SetActive(true);
            LeanTween.textColor(periodMessage.rectTransform, Color.white, 1f).setLoopPingPong(-1);
        }
    }

    void Encount()
    {
        ChromaticAberration(0, 1f, 0.3f);

        Protocol.Send(new BattleBeginSend { guid = stageInfo.guid }, (r) =>
        {
            Window.Open<BattleWindow>(r);
            Observer.Instance.Subscribe(BattleWindow.CloseEvent, OnSubscribe);

            foreach (var player in players)
            {
                player.gameObject.SetActive(false);
            }
        }, (error) =>
        {
            // エラー処理
            return false;
        });
    }


    /// <summary>
    /// 画面エフェクト：バトル出入り
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="time"></param>
    void ChromaticAberration(float from, float to, float time)
    {
        var postProcessing = Camera.main.GetComponent<PostProcessingBehaviour>();
        //postProcessing.profile.chromaticAberration.enabled = true;
        LeanTween.value(from, to, time).setOnUpdate((float v) =>
        {
            var settings = postProcessing.profile.chromaticAberration.settings;
            settings.intensity = v;
            postProcessing.profile.chromaticAberration.settings = settings;
        });
    }

    /// <summary>
    /// Grid情報からワールド座標に変換
    /// </summary>
    /// <param name="grid"></param>
    /// <param name="size"></param>
    /// <param name="offset"></param>
    /// <returns></returns>
    static Vector3 Grid2WorldPosition(Vector2Int grid, Vector2 size, Vector3 offset)
    {
        return new Vector3(grid.x * size.x, 0, -grid.y * size.y) + offset;
    }

    /// <summary>
    /// ワールド座標からGrid座標に変換
    /// </summary>
    /// <param name="position"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    static Vector2Int WorldPosition2Grid(Vector3 position, Vector2 size)
    {
        return Vector2Int.CeilToInt(new Vector2(position.x / size.x, -position.z / size.y));
    }
}
