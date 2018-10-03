///==============================
/// ユニットデータ一覧
///==============================
using System.Collections.Generic;
using Event;

namespace Entities
{
    public class Units
    {
        public const string UpdateEvent = @"Units:Update";

        public List<UnitItem> items { get; private set; }
        public Units(UnitItem[] items)
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
    }
}

