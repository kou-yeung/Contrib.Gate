///==============================
/// ユニットデータ一覧
///==============================
using System.Collections.Generic;

namespace Entities
{
    public class Units
    {
        public List<UnitItem> items { get; private set; }
        public Units(UnitItem[] items)
        {
            this.items = new List<UnitItem>(items);
        }
        /// <summary>
        /// 変更する
        /// </summary>
        /// <param name="egg"></param>
        public void Modify(UnitItem unit)
        {
            items[unit.id] = unit;
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
                Modify(unit);
            }
        }
    }
}

