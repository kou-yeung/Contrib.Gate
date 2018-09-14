using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CsvHelper;
using System.IO;
using System.Linq;
using System;
using Network;
using Security;

namespace Entities
{
    public class Entity
    {
        static Entity instance;
        public static Entity Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Entity();
                    instance.Load();
                }
                return instance;
            }
        }

        // csv データ:ゲーム中変化なし
        public Familiar[] Familiars { get; private set; }
        public Material[] Materials { get; private set; }
        public Vending[] Vendings { get; private set; }
        public Recipe[] Recipes { get; private set; }
        public Item[] items { get; private set; }
        public Cheat[] cheats { get; private set; }
        public UserState userState { get; private set; }

        // 受信データなど、サーバ側キャッシュしたデータ（ローカル更新による疑似的同期を行う
        public Inventory inventory { get; private set; }

        void Load()
        {
            Familiars = Parse<Familiar>("Entities/familiar");
            Materials = Parse<Material>("Entities/material");
            Vendings = Parse<Vending>("Entities/vending");
            Recipes = Parse<Recipe>("Entities/recipe");
            items = Parse<Item>("Entities/item");
            cheats = Parse<Cheat>("Entities/cheat");
        }

        public static string Name(Identify identify)
        {
            switch (identify.Type)
            {
                case IDType.Material:
                    return Array.Find(Instance.Materials, (v) => v.Identify == identify).Name;
                case IDType.Familiar:
                    return Array.Find(Instance.Familiars, (v) => v.Identify == identify).Name;
                case IDType.Item:
                    return Array.Find(Instance.items, (v) => v.Identify == identify).Name;
            }
            return "";
        }

        T[] Parse<T>(string fn)
        {
            var str = Crypt.Decrypt(Resources.Load<TextAsset>(fn).text).Trim();
            using (var csv = new CsvReader(new StringReader(str), CsvHelperRegister.configuration))
            {
                return csv.GetRecords<T>().ToArray();
            }
        }

        public IEnumerator GetInventory(bool refresh = false)
        {
            if (!refresh && inventory != null) yield break;

            bool wait = true;
            Protocol.Send(new InventorySend(), (r) =>
            {
                inventory = new Inventory(r.items);
                wait = false;
            });
            while (wait) yield return null;
        }

        public void UpdateUserState(UserState userState)
        {
            this.userState = userState;
        }
    }
}
