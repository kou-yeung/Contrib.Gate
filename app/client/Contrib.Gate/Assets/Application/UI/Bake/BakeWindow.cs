using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xyz.AnzFactory.UI;
using Entities;
using Network;
using System.Linq;

namespace UI
{
    public class BakeWindow : Window, ANZCellView.IDataSource, ANZCellView.IActionDelegate
    {
        public ANZCellView cell;
        public GameObject bakeItemPrefab;

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

        public void TapCellItem(int index, GameObject listItem)
        {
            var item = listItem.GetComponent<BakeItem>();
            var recipe = item.recipe;

            if (item.valid)
            {
                var msg = recipe.Materials.Select(m => $"{Entity.Name(m.Item1)} x {m.Item2}");

                DialogWindow.OpenYesNo($"{Entity.Name(recipe.Result)} を製作します", string.Join("\n",msg.ToArray()), () =>
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
                    });
                });
            }
            else
            {
                DialogWindow.OpenOk("確認", Entity.Instance.StringTable.Get(ErrorCode.MaterialLack));
            }
            Debug.Log(index);
        }

        protected override void OnStart()
        {
            cell.DataSource = this;
            cell.ActionDelegate = this;
            cell.ReloadData();
            base.OnStart();
        }
    }
}
