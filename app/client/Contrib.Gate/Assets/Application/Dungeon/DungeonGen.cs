using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Dungeon
{
    /// <summary>
    /// 0,1,2
    /// 3,4,5
    /// 6,7,8
    /// </summary>
    [Flags]
    public enum Tile
    {
        None = 0,
        UP = 1 << 0,
        Down = 1 << 1,
        Left = 1 << 2,
        Right = 1 << 3,
        All = UP | Down | Left | Right,
        LeftUpCorner = 1 << 4,
        RightUpCorner = 1 << 5,
        LeftDownCorner = 1 << 6,
        RightDownCorner = 1 << 7,
    }

    class Passage
    {
        Tuple<Room, Room> rooms;

        public Passage(Room room1, Room room2)
        {
            if (room1.Id < room2.Id)
            {
                rooms = Tuple.Create(room1, room2);
            }
            else
            {
                rooms = Tuple.Create(room2, room1);
            }
        }
        public override int GetHashCode()
        {
            return rooms.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj is Passage)
            {
                var o = obj as Passage;
                return o.rooms.Item1 == this.rooms.Item1 && o.rooms.Item2 == this.rooms.Item2;
            }
            return base.Equals(obj);
        }
        public bool Has(Room room)
        {
            return room == rooms.Item1 || room == rooms.Item2;
        }
        public Room GetConnectTo(Room room)
        {
            if (room == rooms.Item1) return rooms.Item2;
            return rooms.Item1;
        }

        public List<Vector2Int> Road(System.Random random)
        {
            var roadWidth = 1;
            List<Vector2Int> pos = new List<Vector2Int>();
            if (rooms.Item1.Grid.y == rooms.Item2.Grid.y)
            {
                // 横接続
                var xMin = rooms.Item1.Area.xMax;   // 左
                var xMax = rooms.Item2.Area.x;      // 右
                var xCenter = (xMax + xMin) / 2;    // 中央

                var y1 = random.Next(-rooms.Item1.Area.height / 4, rooms.Item1.Area.height / 4) + (int)rooms.Item1.Area.center.y;
                var y2 = random.Next(-rooms.Item2.Area.height / 4, rooms.Item2.Area.height / 4) + (int)rooms.Item2.Area.center.y;

                // 左部屋から中間までの道
                for (int i = xMin; i <= xCenter + roadWidth; i++)
                {
                    for (int y = y1 - roadWidth; y <= y1 + roadWidth; y++)
                    {
                        pos.Add(new Vector2Int(i, y));
                    }
                }
                // 左部屋から中間までの道
                for (int i = xMax - 1; i >= xCenter - roadWidth; i--)
                {
                    for (int y = y2 - roadWidth; y <= y2 + roadWidth; y++)
                    {
                        pos.Add(new Vector2Int(i, y));
                    }
                }

                // 連結
                for (int y = Math.Min(y1, y2); y < Math.Max(y1, y2); ++y)
                {
                    for (int x = xCenter - roadWidth; x <= xCenter + roadWidth; x++)
                    {
                        pos.Add(new Vector2Int(x, y));
                    }
                }
            }
            else
            {
                // 縦接続
                var yMin = rooms.Item1.Area.yMax;   // 上
                var yMax = rooms.Item2.Area.y;      // 下
                var yCenter = (yMax + yMin) / 2;    // 中央

                var x1 = random.Next(-rooms.Item1.Area.width / 4, rooms.Item1.Area.width / 4) + (int)rooms.Item1.Area.center.x;
                var x2 = random.Next(-rooms.Item2.Area.width / 4, rooms.Item2.Area.width / 4) + (int)rooms.Item2.Area.center.x;

                // 左部屋から中間までの道
                for (int i = yMin; i <= yCenter + roadWidth; i++)
                {
                    for (int x = x1 - roadWidth; x <= x1+ roadWidth; x++)
                    {
                        pos.Add(new Vector2Int(x, i));
                    }
                }
                // 左部屋から中間までの道
                for (int i = yMax - 1; i >= yCenter - roadWidth; i--)
                {
                    for (int x = x2- roadWidth; x <= x2+ roadWidth; x++)
                    {
                        pos.Add(new Vector2Int(x, i));
                    }
                }

                // 連結
                for (int x = Math.Min(x1, x2); x < Math.Max(x1, x2); ++x)
                {
                    for (int y = yCenter- roadWidth; y <= yCenter + roadWidth; y++)
                    {
                        pos.Add(new Vector2Int(x, y));
                    }
                }
            }
            return pos;
        }

        public bool Merge(System.Random random)
        {
            List<Vector2Int> pos = new List<Vector2Int>();
            if (rooms.Item1.Grid.y == rooms.Item2.Grid.y)
            {
                // 横接続
                var xMin = rooms.Item1.Area.xMax;   // 左
                var xMax = rooms.Item2.Area.x;      // 右
                var xCenter = random.Next(xMax - xMin) + xMin;    // 中央

                rooms.Item1.Area.xMax = xCenter;
                rooms.Item2.Area.xMin = xCenter;

                // 部屋の高さによって重ねてない場合もあるため、確認する
                if (rooms.Item1.Area.yMin < rooms.Item2.Area.yMin && rooms.Item1.Area.yMax > rooms.Item2.Area.yMin)
                {
                    return true;
                }
                if (rooms.Item1.Area.yMin < rooms.Item2.Area.yMax && rooms.Item1.Area.yMax > rooms.Item2.Area.yMax)
                {
                    return true;
                }
                return false; // 重ねてない場合、道は削除しないようにします
            }
            else
            {
                // 縦接続
                var yMin = rooms.Item1.Area.yMax; // 上
                var yMax = rooms.Item2.Area.y;    // 下
                var yCenter = random.Next(yMax - yMin) + yMin;  // 中央

                rooms.Item1.Area.yMax = yCenter;
                rooms.Item2.Area.yMin = yCenter;

                // 部屋の高さによって重ねてない場合もあるため、確認する
                if (rooms.Item1.Area.xMin < rooms.Item2.Area.xMin && rooms.Item1.Area.xMax > rooms.Item2.Area.xMin)
                {
                    return true;
                }
                if (rooms.Item1.Area.xMin < rooms.Item2.Area.xMax && rooms.Item1.Area.xMax > rooms.Item2.Area.xMax)
                {
                    return true;
                }
                return false; // 重ねてない場合、道は削除しないようにします
            }
        }
    }

    class Room
    {
        public int Id;              // 番号
        public Vector2Int Grid;   // Grid座標
        public RectInt Area;    // エリア(範囲)

        public Room(int id, Vector2Int grid, RectInt area)
        {
            this.Id = id;
            this.Grid = grid;
            this.Area = area;
        }
    }


    public static class DungeonGen
    {
#if UNITY_EDITOR
        [UnityEditor.MenuItem("Tools/DungeonGen/Gen")]
        public static void Gen()
        {
            var count = 100;
            for (int i = 0; i < count; i++)
            {
                EditorUtility.DisplayProgressBar("生成中", $"{i}/{count}", i / (float)count);
                var res = Gen(i, new Vector2Int(20, 20), new Vector2Int(3, 3), new Vector2Int(8, 8), new Vector2Int(14, 14));
                Print(res, i.ToString());
            }
            EditorUtility.ClearProgressBar();
        }
#endif
        public static Tile[,] Gen(int seed, Vector2Int areaSize, Vector2Int grid, Vector2Int roomSizeMin, Vector2Int? roomSizeMax = null)
        {
            if (!roomSizeMax.HasValue)
            {
                roomSizeMax = areaSize;
            }
            System.Random random = new System.Random(seed);

            var width = areaSize.x;
            var height = areaSize.y;
            var numX = grid.x;
            var numY = grid.y;

            var flag = new bool[areaSize.x * numX, areaSize.y * numY];
            var rooms = new List<Room>();
            var passages = new List<Passage>();

            for (int x = 0; x < numX; x++)
            {
                for (int y = 0; y < numY; y++)
                {
                    var rect = RandomRect(new RectInt(x * width, y * height, width, height), roomSizeMin, roomSizeMax.Value, random);
                    rooms.Add(new Room(x + y * numX, new Vector2Int(x, y), rect));
                }
            }

            for (int i = 0; i < rooms.Count; i++)
            {
                for (int y = 0; y < rooms.Count; y++)
                {
                    if (i == y) continue;

                    var room1 = rooms[i];
                    var room2 = rooms[y];

                    if ((room1.Grid - room2.Grid).magnitude > 1) continue;

                    var passage = new Passage(room1, room2);
                    if (!passages.Contains(passage))
                    {
                        passages.Add(passage);
                    }
                }
            }

            var deleteRoomTryCount = random.Next((grid.x * grid.y) / 2);    // MEMO : CSV から調整できるようにします
            var deleteRoadTryCount = random.Next((grid.x * grid.y) / 2);    // MEMO : CSV から調整できるようにします
            var mergeRoomRoomTryCount = random.Next(3);                     // MEMO : CSV から調整できるようにします

            DeleteRoom(rooms, passages, deleteRoomTryCount, random);
            DeleteRoad(rooms, passages, deleteRoadTryCount, random);
            MergeRoom(rooms, passages, mergeRoomRoomTryCount, random);

            // 画像出力:部屋
            foreach (var room in rooms)
            {
                var area = room.Area;
                for (int x = area.xMin; x < area.xMax; x++)
                {
                    for (int y = area.yMin; y < area.yMax; y++)
                    {
                        flag[x, y] = true;
                    }
                }
            }
            // 画像出力:道
            foreach (var passage in passages)
            {
                foreach (var road in passage.Road(random))
                {
                    flag[road.x, road.y] = true;
                }
            }
            return CalcTile(flag);
        }

        /// <summary>
        /// 部屋をランダムで削除する
        /// </summary>
        /// <param name="rooms"></param>
        /// <param name="passages"></param>
        /// <param name="tryCount"></param>
        /// <param name="random"></param>
        static void DeleteRoom(List<Room> rooms, List<Passage> passages, int tryCount, System.Random random)
        {
            for (int i = 0; i < tryCount; i++)
            {
                var index = random.Next(rooms.Count);
                var room = rooms[index];
                var tempPassages = passages.Where(v => v.Has(room)).ToArray();

                rooms.RemoveAt(index);
                foreach (var p in tempPassages) passages.Remove(p);

                if (!IsAllConnect(rooms, passages))
                {
                    // 分断されたため戻します
                    rooms.Add(room);
                    foreach (var p in tempPassages) passages.Add(p);
                }
            }
        }

        /// <summary>
        /// 道をランダムで削除
        /// </summary>
        /// <param name="rooms"></param>
        /// <param name="passages"></param>
        /// <param name="tryCount"></param>
        /// <param name="random"></param>
        static void DeleteRoad(List<Room> rooms, List<Passage> passages, int tryCount, System.Random random)
        {
            for (int i = 0; i < tryCount; i++)
            {
                var index = random.Next(passages.Count);
                var passage = passages[index];
                passages.RemoveAt(index);
                if (!IsAllConnect(rooms, passages))
                {
                    // 分断されたため戻します
                    passages.Add(passage);
                }
            }
        }

        /// <summary>
        /// 部屋をマージする
        /// </summary>
        /// <param name="rooms"></param>
        /// <param name="passages"></param>
        /// <param name="tryCount"></param>
        /// <param name="random"></param>
        static void MergeRoom(List<Room> rooms, List<Passage> passages, int tryCount, System.Random random)
        {
            for (int i = 0; i < tryCount; i++)
            {
                var index = random.Next(passages.Count);
                var passage = passages[index];
                if (passage.Merge(random))
                {
                    // 重ねてる場合のみ道を削除する
                    passages.Remove(passage);
                }
            }
        }

        static bool IsAllConnect(List<Room> rooms, List<Passage> passages)
        {
            var hash = new HashSet<Room>();
            IsAllConnect(rooms.First(), passages, hash);
            return rooms.Count == hash.Count;
        }

        static void IsAllConnect(Room room, List<Passage> passages, HashSet<Room> connect)
        {
            if (!connect.Contains(room))
            {
                connect.Add(room);
                foreach (var passage in passages.Where(v => v.Has(room)))
                {
                    IsAllConnect(passage.GetConnectTo(room), passages, connect);
                }
            }
        }

        static void Print(bool[,] flag, string fn)
        {
            List<Color> colors = new List<Color>();
            var width = flag.GetLength(0);
            var height = flag.GetLength(1);
            var tex = new Texture2D(width, height, TextureFormat.ARGB32, false);

            for (int y = height - 1; y >= 0; y--)
            {
                for (int x = 0; x < width; x++)
                {
                    colors.Add(flag[x, y] ? Color.white : Color.black);
                }
            }

            tex.SetPixels(colors.ToArray());
            tex.Apply();
            File.WriteAllBytes($"Dungeon/{fn}.png", tex.EncodeToPNG());
        }

        static void Print(Tile[,] flag, string fn)
        {
            List<Color> colors = new List<Color>();
            var width = flag.GetLength(0);
            var height = flag.GetLength(1);
            var tex = new Texture2D(width, height, TextureFormat.ARGB32, false);

            for (int y = height - 1; y >= 0; y--)
            {
                for (int x = 0; x < width; x++)
                {
                    switch ((int)flag[x,y])
                    {
                        case 0:
                            colors.Add(Color.black);
                            break;
                        case (int)Tile.All:
                            colors.Add(Color.white);
                            break;
                        case (int)Tile.UP:
                            colors.Add(Color.blue);
                            break;
                        case (int)Tile.Down:
                            colors.Add(Color.green);
                            break;
                        case (int)Tile.Left:
                            colors.Add(Color.yellow);
                            break;
                        case (int)Tile.Right:
                            colors.Add(Color.magenta);
                            break;
                        default:
                            colors.Add(Color.red);
                            break;
                    }
                }
            }

            tex.SetPixels(colors.ToArray());
            tex.Apply();
            File.WriteAllBytes($"Dungeon/{fn}.png", tex.EncodeToPNG());
        }

        static Tile[,] CalcTile(bool[,] flag)
        {
            var width = flag.GetLength(0);
            var height = flag.GetLength(1);
            var tiles = new Tile[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (flag[x, y])
                    {
                        Tile tile = Tile.None;
                        var left = x - 1;
                        var right = x + 1;
                        var up = y - 1;
                        var down = y + 1;

                        if (left >= 0 && flag[left, y]) tile |= Tile.Right;
                        if (right < width && flag[right, y]) tile |= Tile.Left;
                        if (up >= 0 && flag[x, up]) tile |= Tile.Down;
                        if (down < height && flag[x, down]) tile |= Tile.UP;

                        if (tile == Tile.All)
                        {
                            if (left >= 0 && up >= 0 && !flag[left, up])
                            {
                                tile = Tile.LeftUpCorner;
                            }
                            else if (left >= 0 && down < height && !flag[left, down])
                            {
                                tile = Tile.LeftDownCorner;
                            }
                            else if (right < width && up >= 0 && !flag[right, up])
                            {
                                tile = Tile.RightUpCorner;
                            }
                            else if (right < width && down < height && !flag[right, down])
                            {
                                tile = Tile.RightDownCorner;
                            }
                        }

                        tiles[x, y] = tile;
                    }
                    else
                    {
                        tiles[x, y] = Tile.None;
                    }
                }
            }
            return tiles;
        }

        static RectInt RandomRect(RectInt area, Vector2Int roomSizeMin, Vector2Int roomSizeMax, System.Random random)
        {
            // 部屋サイズ
            int w = Math.Min(random.Next(area.width - roomSizeMin.x) + roomSizeMin.x, roomSizeMax.x);
            int h = Math.Min(random.Next(area.height - roomSizeMin.y) + roomSizeMin.y, roomSizeMax.y);

            // 部屋座標
            int sx = random.Next(area.width - w) + (area.x);
            int sy = random.Next(area.height - h) + (area.y);

            return new RectInt(sx, sy, w, h);
        }
    }
}

//// IEnumerable に拡張メソッド定義
//public static class IEnumerableExtension
//{
//    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> collection)
//    {
//        return collection.OrderBy(i => Guid.NewGuid());
//    }
//}