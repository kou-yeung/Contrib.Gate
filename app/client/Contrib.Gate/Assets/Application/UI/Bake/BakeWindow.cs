using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xyz.AnzFactory.UI;
using Entities;
using Network;
using System.Linq;
using UnityEngine.UI;
using System;

namespace UI
{
    public class BakeWindow : Window, ANZCellView.IDataSource, ANZCellView.IActionDelegate, ANZCellView.IPressDelegate
    {
        public ANZCellView cell;
        public GameObject bakeItemPrefab;
        public BakeItem currentBakeItem;
        // 詳細
        public Text recipeName;
        public Text[] materials;
        public Text desc;
        public Button production;

        public GameObject CellViewItem(int index, GameObject item)
        {
            if (item == null) item = Instantiate(bakeItemPrefab);
            // データ設定
            item.GetComponent<BakeItem>().Setup(Entity.Instance.Recipes[index]);
            return item;
        }

        public Vector2 ItemSize()
        {
            return bakeItemPrefab.GetComponent<RectTransform>().sizeDelta;
        }

        public int NumOfItems()
        {
            return Entity.Instance.Recipes.Length;
        }

        public void PressCellItem(int index, GameObject listItem)
        {
            Debug.Log($"PressCellItem {index}");
        }

        public void TapCellItem(int index, GameObject listItem)
        {
            currentBakeItem = listItem.GetComponent<BakeItem>();
            Detail(currentBakeItem.recipe);
        }

        protected override void OnStart()
        {
            cell.DataSource = this;
            cell.ActionDelegate = this;
            cell.PressDelegate = this;
            cell.ReloadData();
            base.OnStart();
        }

        protected override void OnButtonClick(Button btn)
        {
            switch (btn.name)
            {
                case "Production":
                    Production();
                    break;
                default:
                    base.OnButtonClick(btn);
                    break;
            }
        }

        /// <summary>
        /// 制作
        /// </summary>
        public void Production()
        {
            var recipe = currentBakeItem.recipe;
            var msg = recipe.Materials.Select(m => $"{Entity.Name(m.Item1)} x {m.Item2}");

            DialogWindow.OpenYesNo($"{Entity.Name(recipe.Result)} を製作します", string.Join("\n", msg.ToArray()), () =>
             {
                 Protocol.Send(new RecipeSend { identify = recipe.Identify }, (r) =>
                 {
                     // 交換しました、キャッシュした情報を更新します

                     // 消費したアイテムを減らし
                     foreach (var mat in recipe.Materials)
                     {
                         Entity.Instance.Inventory.Add(mat.Item1, -mat.Item2);
                     }
                     // 獲得したものを追加します
                     Entity.Instance.Inventory.Add(r.identify, 1);

                     // セル更新する
                     cell.ReloadData();

                     // 詳細更新
                     Detail(currentBakeItem.recipe);
                 });
             });
        }

        /// <summary>
        /// 詳細設定
        /// </summary>
        public void Detail(Recipe recipe)
        {
            recipeName.text = Entity.Name(recipe.Result);
            var inventory = Entity.Instance.Inventory;

            for (int i = 0; i < materials.Length; i++)
            {
                if (i < recipe.Materials.Count)
                {
                    var mat = recipe.Materials[i];
                    var has = inventory.Count(mat.Item1);
                    materials[i].text = $"{Entity.Name(mat.Item1)} x {mat.Item2} ({has})";
                    materials[i].color = (mat.Item2 <= has) ? Color.black : Color.red;
                }
                else
                {
                    materials[i].text = "";
                }
            }
            production.interactable = currentBakeItem.valid;
            desc.text = Entity.Desc(recipe.Result);
        }
    }
}
