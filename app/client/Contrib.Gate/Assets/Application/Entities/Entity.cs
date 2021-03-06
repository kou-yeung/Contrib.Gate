﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CsvHelper;
using System.IO;
using System.Linq;
using System;
using Network;
using Security;
using EventSystem;

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

        public float[,] ParamTable = new float[,]
        {
            {8.0f, 2.0f, 3.0f, 3.0f, 1.0f},     // HP
            {1.0f, 2.0f, 2.0f, 2.0f, 10.0f},    // MP
            {0.1f, 2.0f, 0.2f, 0.2f, 0.1f},     // Attack
            {0.1f, 0.2f, 2.0f, 0.2f, 0.1f},     // Defense
            {0.1f, 0.2f, 0.2f, 2.0f, 0.1f},     // Agility
            {-.1f, -.1f, 0.2f, -.1f, 3.0f},     // Magic
        };

        public int CalcParam(Param param, int[] baseParam, int[] addParam)
        {
            var res = .0f;
            for (int i = 0; i < (int)Score.Count; i++)
            {
                res += (baseParam[i] + addParam[i]) * ParamTable[(int)param, i];
            }
            return Mathf.Max(0, Mathf.FloorToInt(res));
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
        public Enemy[] Enemies { get; private set; }
        public LevelTable LevelTable { get; private set; }
        public Skill[] Skills { get; private set; }
        public Configs Configs { get; private set; }

        // 受信データなど、サーバ側キャッシュしたデータ（ローカル更新による疑似的同期を行う
        public Inventory Inventory { get; private set; }
        public UserState UserState { get; private set; }
        public StringTable StringTable { get; private set; }
        public StageInfo StageInfo { get; private set; }
        public EggList EggList { get; private set; }
        public PetList PetList { get; private set; }
        public HatchList HatchList { get; private set; }
        public UnitList UnitList { get; private set; }
        public StageList StageList { get; private set; }
        public BinderList BinderList { get; private set; }

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
            Enemies = Parse<Enemy>("Entities/enemy");
            Skills = Parse<Skill>("Entities/skill");

            var levels = Parse<Level>("Entities/level");
            if (levels != null) LevelTable = new LevelTable(levels);

            Cheats = Parse<Cheat>("Entities/cheat", false);
            StringTable = new StringTable(Parse<StringTableKV>("Entities/string_table", false));
            Configs = Parse<Configs>("Entities/config", false).FirstOrDefault();
        }

        /// <summary>
        /// 名前
        /// </summary>
        /// <param name="identify"></param>
        /// <returns></returns>
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
                case IDType.Skill:
                    return Array.Find(Instance.Skills, (v) => v.Identify == identify).Name;
                case IDType.Stage:
                    return Array.Find(Instance.Stages, (v) => v.Identify == identify).Name;
            }
            return "";
        }

        /// <summary>
        /// 説明文
        /// </summary>
        /// <param name="identify"></param>
        /// <returns></returns>
        public static string Desc(Identify identify)
        {
            switch (identify.Type)
            {
                case IDType.Item:
                    return Array.Find(Instance.Items, (v) => v.Identify == identify).Desc;
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

        // インベントリ取得
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
        // タマゴ一覧取得
        public IEnumerator GetEggList(bool refresh = false)
        {
            if (!refresh && EggList != null) yield break;
            bool wait = true;
            Protocol.Send(new EggListSend(), (r) =>
            {
                EggList = new EggList(r.items);
                wait = false;
            });
            while (wait) yield return null;
        }
        // ペット一覧取得
        public IEnumerator GetPetList(bool refresh = false)
        {
            if (!refresh && PetList != null) yield break;
            bool wait = true;
            Protocol.Send(new PetListSend(), (r) =>
            {
                PetList = new PetList(r.items);
                wait = false;
            });
            while (wait) yield return null;
        }

        // 孵化一覧取得
        public IEnumerator GetHatchList(bool refresh = false)
        {
            if (!refresh && HatchList != null) yield break;
            bool wait = true;
            Protocol.Send(new HatchListSend(), (r) =>
            {
                HatchList = new HatchList(r.items);
                wait = false;
            });
            while (wait) yield return null;
        }
        // ユニット一覧取得
        public IEnumerator GetUnitList(bool refresh = false)
        {
            if (!refresh && UnitList != null) yield break;
            bool wait = true;
            Protocol.Send(new UnitListSend(), (r) =>
            {
                UnitList = new UnitList(r.items);
                wait = false;
            });
            while (wait) yield return null;
        }
        // ステージ一覧取得
        public IEnumerator GetStageList(bool refresh = false)
        {
            if (!refresh && StageList != null)
            {
                StageList.LocalUpdatePeriod();  // ローカル更新
                yield break;
            }
            bool wait = true;
            Protocol.Send(new StageListSend(), (r) =>
            {
                StageList = new StageList(r.items, r.period);
                wait = false;
            });
            while (wait) yield return null;
        }
        // 図鑑一覧取得
        public IEnumerator GetBinder(bool refresh = false)
        {
            if (!refresh && BinderList != null) yield break;
            bool wait = true;
            Protocol.Send(new BinderListSend(), (r) =>
            {
                BinderList = new BinderList(r.ids);
                wait = false;
            });
            while (wait) yield return null;
        }


        public void UpdateUserState(UserState userState)
        {
            this.UserState = userState;
            Observer.Instance.Notify(UserState.Update.ToString(), userState);
        }
        public void UpdateUserState(UserState[] userState)
        {
            if (userState == null) return;
            foreach (var s in userState)
            {
                UpdateUserState(s);
            }
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
