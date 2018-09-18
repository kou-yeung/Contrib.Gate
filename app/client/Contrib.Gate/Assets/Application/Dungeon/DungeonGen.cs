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
    enum Tile
    {
    }

    class Road
    {
        public Vector2Int start;
        public Vector2Int end;
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
            List<Vector2Int> pos = new List<Vector2Int>();
            if (rooms.Item1.Grid.y == rooms.Item2.Grid.y)
            {
                // 横接続
                var xMin = rooms.Item1.Area.xMax;   // 左
                var xMax = rooms.Item2.Area.x;      // 右
                var xCenter = (xMax + xMin) / 2;    // 中央

                var y1 = random.Next(rooms.Item1.Area.height) + rooms.Item1.Area.y;
                var y2 = random.Next(rooms.Item2.Area.height) + rooms.Item2.Area.y;

                // 左部屋から中間までの道
                for (int i = xMin; i <= xCenter; i++)
                {
                    pos.Add(new Vector2Int(i, y1));
                }
                // 左部屋から中間までの道
                for (int i = xMax - 1; i >= xCenter; i--)
                {
                    pos.Add(new Vector2Int(i, y2));
                }

                // 連結
                for (int y = Math.Min(y1, y2); y < Math.Max(y1, y2); ++y)
                {
                    pos.Add(new Vector2Int(xCenter, y));
                }
            }
            else
            {
                // 縦接続
                var yMin = rooms.Item1.Area.y + rooms.Item1.Area.height;    // 上
                var yMax = rooms.Item2.Area.y;    // 下
                var yCenter = (yMax + yMin) / 2;  // 中央

                var x1 = random.Next(rooms.Item1.Area.width) + rooms.Item1.Area.x;
                var x2 = random.Next(rooms.Item2.Area.width) + rooms.Item2.Area.x;

                // 左部屋から中間までの道
                for (int i = yMin; i <= yCenter; i++)
                {
                    pos.Add(new Vector2Int(x1, i));
                }
                // 左部屋から中間までの道
                for (int i = yMax - 1; i >= yCenter; i--)
                {
                    pos.Add(new Vector2Int(x2, i));
                }

                // 連結
                for (int x = Math.Min(x1, x2); x < Math.Max(x1, x2); ++x)
                {
                    pos.Add(new Vector2Int(x, yCenter));
                }
            }
            return pos;
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
            Gen(new Vector2Int(30, 30), new Vector2Int(3, 3), new Vector2Int(12, 20));
        }
#endif
        public static void Gen(Vector2Int areaSize, Vector2Int grid, Vector2Int roomSizeMin)
        {
            for (int p = 0; p < 100; p++)
            {
                EditorUtility.DisplayProgressBar("生成中", $"{p}/{100}", p / 100f);
                System.Random random = new System.Random(p);

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
                        var rect = RandomRect(new RectInt(x * width, y * height, width, height), roomSizeMin, random);
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

                DeleteRoom(rooms, passages, deleteRoomTryCount, random);
                DeleteRoad(rooms, passages, deleteRoadTryCount, random);

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

                Print(flag, p.ToString());
            }
            EditorUtility.ClearProgressBar();
        }

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

        static RectInt RandomRect(RectInt area, Vector2Int roomSizeMin, System.Random random)
        {
            // 部屋サイズ
            int w = random.Next(area.width - roomSizeMin.x) + roomSizeMin.x;
            int h = random.Next(area.height - roomSizeMin.y) + roomSizeMin.y;

            // 部屋座標
            int sx = random.Next(area.width - w) + (area.x + 1);
            int sy = random.Next(area.height - h) + (area.y + 1);

            return new RectInt(sx, sy, w, h);
        }
    }
}
// IEnumerable に拡張メソッド定義
public static class IEnumerableExtension
{
    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> collection)
    {
        return collection.OrderBy(i => Guid.NewGuid());
    }
}