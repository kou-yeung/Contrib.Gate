using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Entities;

namespace UI
{
    public class DebugItem : MonoBehaviour
    {
        public Cheat cheat { get; private set; }
        public new Text name;

        public void Setup(Cheat cheat)
        {
            this.cheat = cheat;
            this.name.text = cheat.Name;
        }
    }
}
