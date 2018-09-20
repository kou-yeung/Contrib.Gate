using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CsvHelper;
using System.IO;
using System.Linq;
using System;
using Network;
using Security;
using Event;

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
        public Item[] Items { get; private set; }
        public Cheat[] Cheats { get; private set; }
        public Dungeon[] Dungeons { get; private set; }
        public Room[] Rooms { get; private set; }
        public Stage[] Stages { get; private set; }

        // 受信データなど、サーバ側キャッシュしたデータ（ローカル更新による疑似的同期を行う
        public Inventory Inventory { get; private set; }
        public UserState UserState { get; private set; }
        public StringTable StringTable { get; private set; }
        public StageInfo StageInfo { get; private set; }

        public void Load()
        {
            Familiars = Parse<Familiar>("Entities/familiar");
            Materials = Parse<Material>("Entities/material");
            Vendings = Parse<Vending>("Entities/vending");
            Recipes = Parse<Recipe>("Entities/recipe");
            Items = Parse<Item>("Entities/item");
            Dungeons = Parse<Dungeon>("Entities/dungeon");
            Rooms = Parse<Room>("Entities/room");
            Stages = Parse<Stage>("Entities/stage");

            Cheats = Parse<Cheat>("Entities/cheat", false);
            StringTable = new StringTable(Parse<StringTableKV>("Entities/string_table", false));
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
                    return Array.Find(Instance.Items, (v) => v.Identify == identify).Name;
            }
            return "";
        }

        T[] Parse<T>(string fn, bool crypt = true)
        {
            if (crypt && !Crypt.Ready()) return default(T[]);

            var str = Resources.Load<TextAsset>(fn).text;
            if (crypt) str = Crypt.Decrypt(str);

            using (var csv = new CsvReader(new StringReader(str.Trim()), CsvHelperRegister.configuration))
            {
                return csv.GetRecords<T>().ToArray();
            }
        }

        public IEnumerator GetInventory(bool refresh = false)
        {
            if (!refresh && Inventory != null) yield break;

            bool wait = true;
            Protocol.Send(new InventorySend(), (r) =>
            {
                Inventory = new Inventory(r.items);
                wait = false;
            });
            while (wait) yield return null;
        }

        public void UpdateUserState(UserState userState)
        {
            this.UserState = userState;
            Observer.Instance.Notify(UserState.Update.ToString(), userState);
        }

        /// <summary>
        /// ユーザが遊んでるステージの情報
        /// </summary>
        /// <param name="stageInfo"></param>
        public void UpdateStageInfo(StageInfo stageInfo)
        {
            this.StageInfo = stageInfo;
        }
    }
}
