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
        Score type;

        public void Setup(Score type, Entities.PetItem pet, int value)
        {
            this.type = type;
            paramType.text = Entity.Instance.StringTable.Get(type);
            var powerup = pet.param[(int)type];

            if (value != 0)
            {
                paramValue.text = $"{powerup + value} (+{value})";
                paramValue.color = Color.red;
            }
            else
            {
                paramValue.text = $"{powerup + value}";
                paramValue.color = Color.black;
            }

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
