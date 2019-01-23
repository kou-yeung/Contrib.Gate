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
        Param type;

        public void Setup(Param type)
        {
            this.type = type;
            paramType.text = type.ToString();
            paramValue.text = $"{200} ({5})";
        }

        public void OnClick(Button btn)
        {
            switch (btn.name)
            {
                case "Up":
                    //Debug.Log($"{type} : Up");
                    Observer.Instance.Notify(PowerupChangeEvent, $"{type}:+");
                    break;
                case "Down":
                    //Debug.Log($"{type} : Down");
                    Observer.Instance.Notify(PowerupChangeEvent, $"{type}:-");
                    break;
            }
        }

        public void OnEndEdit(InputField input)
        {
            uint value;
            uint.TryParse(input.text, out value);
            Debug.Log($"{type} : {value}");
            input.text = value.ToString();

            Observer.Instance.Notify(PowerupChangeEvent, $"{type}:{value}");
        }
    }
}
