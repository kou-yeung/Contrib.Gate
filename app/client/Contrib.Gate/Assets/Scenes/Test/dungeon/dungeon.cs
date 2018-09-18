using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dungeon;

namespace Test
{
    public class dungeon : MonoBehaviour
    {
        void Start()
        {
            DungeonGen.Gen(0, new Vector2Int(45, 45), new Vector2Int(3, 3), new Vector2Int(12, 12));
        }

        void Update()
        {

        }
    }
}
