///==============================
/// 図鑑データ一覧
///==============================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities
{
    public class BinderList
    {
        HashSet<Identify> ids;

        public BinderList(string[] ids)
        {
            this.ids = new HashSet<Identify>();
            foreach (var id in ids)
            {
                this.ids.Add(Identify.Parse(id));
            }
        }

        /// <summary>
        /// 変更する
        /// </summary>
        /// <param name="id"></param>
        public void Modify(Identify id)
        {
            ids.Add(id);
        }
    }
}
