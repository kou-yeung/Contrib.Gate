using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Entities;

namespace UI
{
    public class BakeItem : MonoBehaviour
    {
        public new Text name;
        public Text[] materials;
        public Recipe recipe { get; private set; }
        public bool valid { get; private set; } // 実行可能か？

        public void Setup(Recipe recipe)
        {
            this.recipe = recipe;
            valid = true;

            name.text = Entity.Name(recipe.Result);
            var inventory = Entity.Instance.Inventory;
            for (int i = 0; i < materials.Length; i++)
            {
                if (i < recipe.Materials.Count)
                {
                    var mat = recipe.Materials[i];
                    var item = inventory.Find(mat.Item1);
                    var has = 0;
                    if (item != null) has = item.num;
                    materials[i].text = $"{Entity.Name(mat.Item1)} x {mat.Item2} ({has})";
                    materials[i].color = (has >= mat.Item2) ? Color.black:Color.red;
                    valid &= (has >= mat.Item2); // 交換
                }
                else
                {
                    materials[i].text = "";
                }
            }
        }
    }
}
