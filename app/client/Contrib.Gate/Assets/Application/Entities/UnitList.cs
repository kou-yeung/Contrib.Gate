///==============================
/// ユニットデータ一覧
///==============================
using System.Collections.Generic;
using Event;
using System.Linq;
using System;
using UnityEngine;

namespace Entities
{
    public class UnitList
    {
        public const string UpdateEvent = @"Units:Update";

        public List<UnitItem> items { get; private set; }
        public UnitList(UnitItem[] items)
        {
            this.items = new List<UnitItem>(items);
        }
        /// <summary>
        /// 変更する
        /// </summary>
        /// <param name="egg"></param>
        public void Modify(UnitItem unit, bool notify = true)
        {
            items[unit.id] = unit;
            if (notify) Observer.Instance.Notify(UpdateEvent);
        }
        /// <summary>
        /// 変更する
        /// </summary>
        /// <param name="egg"></param>
        public void Modify(UnitItem[] units)
        {
            if (units == null) return;
            foreach (var unit in units)
            {
                Modify(unit, false);
            }
            Observer.Instance.Notify(UpdateEvent);
        }

        /// <summary>
        /// ユニットに配置されている
        /// </summary>
        /// <param name="uniqid"></param>
        /// <returns></returns>
        public bool Exists(string uniqid)
        {
            return items.Exists(item => Array.Exists(item.uniqids, v => v == uniqid));
        }

        /// <summary>
        /// 情報複製:編集時のローカル情報に使用します
        /// </summary>
        /// <returns></returns>
        public UnitList Clone()
        {
            var data = new UnitItem[items.Count];
            for (int i = 0; i < items.Count; i++)
            {
                data[i] = JsonUtility.FromJson<UnitItem>(JsonUtility.ToJson(items[i]));
            }
            var res = new UnitList(data);
            return res;
        }

        /// <summary>
        /// 比較して、変更があれば(true)
        /// </summary>
        /// <param name="units"></param>
        /// <returns></returns>
        public bool IsModify(UnitList units)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].name != units.items[i].name) return true;
                if (!items[i].uniqids.SequenceEqual(units.items[i].uniqids)) return true;
            }
            return false;
        }
    }
}

