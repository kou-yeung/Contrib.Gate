using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entities;
using UnityEngine.UI;
using EventSystem;

namespace UI
{
    public class PowerupItem : MonoBehaviour
    {
        public const string PowerupChangeEvent = @"PowerupItem:Change";

        public Text paramType;
        public Text paramValue;
        public InputField input;
        Param type;

        public void Setup(Param type, Entities.PetItem pet, int value)
        {
            this.type = type;
            paramType.text = Entity.Instance.StringTable.Get(type);

            var powerup = pet.param[(int)type];
            var totalParam = pet.GetParam(type);
            paramValue.text = $"{totalParam + value} ({powerup + value})";
            paramValue.color = (value != 0) ? Color.red : Color.black;

            input.text = $"{value}";
        }

        public void OnClick(Button btn)
        {
            switch (btn.name)
            {
                case "Up":
                    Observer.Instance.Notify(PowerupChangeEvent, $"{type}:+");
                    break;
                case "Down":
                    Observer.Instance.Notify(PowerupChangeEvent, $"{type}:-");
                    break;
            }
        }

        public void OnEndEdit(InputField input)
        {
            uint value;
            uint.TryParse(input.text, out value);
            input.text = value.ToString();
            Observer.Instance.Notify(PowerupChangeEvent, $"{type}:{value}");
        }
    }
}
