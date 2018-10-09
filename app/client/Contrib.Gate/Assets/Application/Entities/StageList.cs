///==============================
/// ステージ一覧
///==============================
using System.Collections.Generic;

namespace Entities
{
    public class StageList
    {
        public List<StageItem> items { get; private set; }
        public long period { get; private set; }

        public StageList(StageItem[] items, long period)
        {
            this.items = new List<StageItem>(items);
            this.period = period;
        }

        /// <summary>
        /// 変更する
        /// </summary>
        public void Modify(StageItem item)
        {
            var index = items.FindIndex(v => v.id == item.id);
            if (index != -1) items[index] = item;
            else items.Add(item);
        }
        /// <summary>
        /// 変更する
        /// </summary>
        /// <param name="egg"></param>
        public void Modify(StageItem[] items)
        {
            if (items == null) return;
            foreach (var item in items)
            {
                Modify(item);
            }
        }

        /// <summary>
        /// ローカルの期間チェック
        /// 過ぎたら2時間を足す
        /// true(更新された) false(まだ有効)
        /// </summary>
        public bool LocalUpdatePeriod()
        {
            if (period <= Util.Time.ServerTime.CurrentUnixTime)
            {
                period += 3600 * 2; //２時間先へ
                return true;
            }
            return false;
        }

        /// <summary>
        /// 指定したステージがピリオト中クリアした
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ClearThisPeriod(Identify id)
        {
            var item = items.Find(v => v.id == id.idWithType);
            if (item == null) return false; // 存在しなかったため一度もクリアしたことない
            return item.clear >= (period - 3600 * 2);

        }

        /// <summary>
        /// 新規ステージ（一回もクリアしたことない）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsNew(Identify id)
        {
            var item = items.Find(v => v.id == id.idWithType);
            return (item == null);
        }

    }
}
