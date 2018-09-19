using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Entities
{
    public class Room
    {
        public Identify Identify;
        public Vector2Int AreaSize; // 1Grid のサイズ
        public Vector2Int RoomNum;  // 部屋数
        public Vector2Int RoomMin;  // 部屋最小サイズ
        public Vector2Int RoomMax;  // 部屋最大サイズ
        public int DeleteRoomTry;   // 部屋削除最大試行回数
        public int DeleteRoadTry;   // 道削除最大試行回数
        public int MergeRoomTry;    // 部屋マージ最大試行回数
    }

    public sealed class RoomMap : ClassMap<Room>
    {
        public RoomMap()
        {
            Map(x => x.Identify).Index(0);
            Map(x => x.AreaSize).Index(1);
            Map(x => x.RoomNum).Index(2);
            Map(x => x.RoomMin).Index(3);
            Map(x => x.RoomMax).Index(4);
            Map(x => x.DeleteRoomTry).Index(5);
            Map(x => x.DeleteRoadTry).Index(6);
            Map(x => x.MergeRoomTry).Index(7);
        }
    }

}
