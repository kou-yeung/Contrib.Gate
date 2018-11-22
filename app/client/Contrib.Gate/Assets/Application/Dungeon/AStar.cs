using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entities;
using SettlersEngine;
using System.Linq;

namespace Dungeon
{
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


    public class AStar
    {
        SpatialAStar<PathNode, object> aStar;

        public AStar(Tile[,] tiles)
        {
            var width = tiles.GetLength(0);
            var height = tiles.GetLength(1);

            var nodes = new PathNode[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    nodes[x, y] = new PathNode { pos = new Vector2Int(x, y), tile = tiles[x, y] };
                }
            }
            aStar = new SpatialAStar<PathNode, object>(nodes);
        }

        /// <summary>
        /// 経路検索
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public List<PathNode> Search(Vector2Int from, Vector2Int to)
        {
            var res = aStar.Search(from, to, null);
            return (res != null) ? res.ToList() : null;
        }
    }
}
