﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dungeon;
using UnityEngine.UI;
using UnityEngine.PostProcessing;

namespace Test
{
    public class dungeon : MonoBehaviour
    {
        Vector2 GridSize = Vector2.one;
        public string assetName = "001";
        public int obstacleRate = 5;

        void Start()
        {
            var mapchip = Resources.Load<MapchipSet>($"Dungeon/{assetName}/MapchipSet");
            GridSize = mapchip.GridSize;

            var postProcessing = Camera.main.GetComponent<PostProcessingBehaviour>();
            if (postProcessing != null) postProcessing.profile = mapchip.PostProcessingProfile;

            var additional = new[] { Tile.UpStairs, Tile.DownStairs };
            var map = DungeonGen.Gen(0, new Vector2Int(30, 30), new Vector2Int(3, 3), new Vector2Int(12, 12), new Vector2Int(20, 20), 2, 2, 1, obstacleRate, additional);

            var width = map.GetLength(0);
            var height = map.GetLength(1);

            var random = new System.Random();
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var info = mapchip.GetChip(map[x, y], random);
                    if (info != null)
                    {
                        var go = Instantiate(info.prefab, this.transform);
                        go.transform.localRotation = info.Quaternion;
                        go.transform.localPosition = new Vector3(x * GridSize.x, go.transform.localPosition.y, -y * GridSize.y);
                    }
                }
            }
        }
    }
}
