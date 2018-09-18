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
            DungeonGen.Gen(150, 150);
        }

        void Update()
        {

        }
    }
}
