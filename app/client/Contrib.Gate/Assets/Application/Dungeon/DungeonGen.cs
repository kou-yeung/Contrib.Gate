using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;

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

    public static class DungeonGen
    {
        public static void Gen(int width, int height)
        {
            for (int i = 0; i < 100; i++)
            {
                var flag = new bool[width*3, height*3];

                System.Random random = new System.Random(i);

                var rects = new Dictionary<Vector2Int, RectInt>();

                /// エリア分割
                for (int rx = 0; rx < 3; rx++)
                {
                    for (int ry = 0; ry < 3; ry++)
                    {
                        var rect = RandomRect(new RectInt(rx * width, ry * height, width, height), 80, 250, random);

                        rects.Add(new Vector2Int(rx, ry), rect);
                    }
                }

                //// 消す
                //{
                //    var num = random.Next(0, 4);
                //    var keys = rects.Keys.ToList();
                //    for (int x = 0; x < num; x++)
                //    {
                //        var index = random.Next(0, keys.Count);
                //        var key = keys[index];
                //        var r = rects[key];
                //        r.x += (r.width / 2) -2;
                //        r.y += (r.height / 2) -2;
                //        r.width = 4;
                //        r.height = 4;
                //        rects[key] = r;
                //    }
                //}

                //// 道を作成
                var roadHash = new List<Tuple<Vector2Int, Vector2Int>>
                {
                    // 横の道
                    Tuple.Create(new Vector2Int(0,0), new Vector2Int(1,0)),
                    Tuple.Create(new Vector2Int(1,0), new Vector2Int(2,0)),

                    Tuple.Create(new Vector2Int(0,1), new Vector2Int(1,1)),
                    Tuple.Create(new Vector2Int(1,1), new Vector2Int(2,1)),

                    Tuple.Create(new Vector2Int(0,2), new Vector2Int(1,2)),
                    Tuple.Create(new Vector2Int(1,2), new Vector2Int(2,2)),

                    // 縦の道
                    Tuple.Create(new Vector2Int(0,0), new Vector2Int(0,1)),
                    Tuple.Create(new Vector2Int(0,1), new Vector2Int(0,2)),

                    Tuple.Create(new Vector2Int(1,0), new Vector2Int(1,1)),
                    Tuple.Create(new Vector2Int(1,1), new Vector2Int(1,2)),

                    Tuple.Create(new Vector2Int(2,0), new Vector2Int(2,1)),
                    Tuple.Create(new Vector2Int(2,1), new Vector2Int(2,2)),
                };

                var road = new List<Road>();
                foreach (var hash in roadHash)
                {
                    var r1 = rects[hash.Item1];
                    var r2 = rects[hash.Item2];

                    if (hash.Item1.x == hash.Item2.x)
                    {
                        var p1 = new Vector2Int(r1.x, random.Next(r1.height) + r1.y);
                        var p2 = new Vector2Int(r2.x, random.Next(r2.height) + r2.y);

                        var c = new Vector2Int( (p1.x + p2.x) / 2, (p1.y + p2.y) / 2);

                        // 横のつながり
                        road.Add(new Road { start = p1, end = new Vector2Int(c.x, p1.y) });
                        road.Add(new Road { start = c, end = new Vector2Int(c.x, p1.y) });

                        road.Add(new Road { start = c, end = new Vector2Int(c.x, p2.y) });
                        road.Add(new Road { start = p2, end = new Vector2Int(p2.x, c.y) });
                    }
                    else
                    {
                        // 縦のつながり
                    }
                }

                foreach (var r in road)
                {
                    if (r.start.x == r.end.x)
                    {
                        //var x = r.start.x;
                        for (int x = r.start.x - 2; x < r.start.x + 2; x++)
                        {
                            // Y 軸
                            for (int y = r.start.y; y < r.end.y; y++)
                            {
                                flag[x, y] = true;
                            }
                        }
                    }
                    else
                    {
                        for (int y = r.start.y-2; y < r.start.y + 2; y++)
                        {
                            // X軸
                            for (int x = r.start.x; x < r.end.x; x++)
                            {
                                flag[x, y] = true;
                            }

                        }
                    }
                }

                //for (int x = 1; x < 3; x++)
                //{
                //    for (int y = 1; y < 3; y++)
                //    {
                //        var start = new Vector2Int(x - 1, y - 1);
                //        var end = new Vector2Int(x, y);


                //    }
                //}


                // 画像出力
                foreach (var kv in rects)
                {
                    var rect = kv.Value;
                    for (int x = rect.xMin; x < rect.xMax; x++)
                    {
                        for (int y = rect.yMin; y < rect.yMax; y++)
                        {
                            flag[x, y] = true;
                        }
                    }
                }

                Print(flag, i.ToString());
            }
        }

        static void Print(bool[,] flag, string fn)
        {
            List<Color> colors = new List<Color>();
            var tex = new Texture2D(flag.GetLength(0), flag.GetLength(1));
            foreach (var f in flag)
            {
                colors.Add(f ? Color.white : Color.black);
            }
            tex.SetPixels(colors.ToArray());
            tex.Apply();
            File.WriteAllBytes($"Dungeon/{fn}.png", tex.EncodeToPNG());
        }

        static RectInt RandomRect(RectInt area, int roomSizeMin, int roomSizeMax, System.Random random)
        {
            // 部屋サイズ
            int w = Math.Min(random.Next(area.width - roomSizeMin) + (roomSizeMin), roomSizeMax);
            int h = Math.Min(random.Next(area.height - roomSizeMin) + (roomSizeMin), roomSizeMax);

            // 部屋座標
            int sx = random.Next(area.width - w) + (area.x + 1);
            int sy = random.Next(area.height - h) + (area.y + 1);

            return new RectInt(sx, sy, w, h);
        }
    }
}
