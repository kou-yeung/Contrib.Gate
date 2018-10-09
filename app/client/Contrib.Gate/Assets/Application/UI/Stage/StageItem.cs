using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entities;
using UnityEngine.UI;
namespace UI
{

    public class StageItem : MonoBehaviour
    {
        public Image icon;
        public new Text name;
        public new Text New;
        public Image clear; // このピリオトはクリアした
        public Stage stage { get; private set; }

        public void Setup(Stage data)
        {
            stage = data;
            name.text = data.Name;
            clear.gameObject.SetActive(Entity.Instance.StageList.ClearThisPeriod(stage.Identify));
            New.gameObject.SetActive(Entity.Instance.StageList.IsNew(stage.Identify));
        }
    }
}
