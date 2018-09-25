using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Entities
{
    public partial class PetItem
    {
        private Familiar _familiar;
        public Familiar Familiar
        {
            get
            {
                if (_familiar == null)
                {
                    _familiar = Array.Find(Entity.Instance.Familiars, v => v.Identify == this.id);
                }
                return _familiar;
            }
        }

        /// <summary>
        /// レベル取得
        /// </summary>
        public int Level
        {
            get { return Entity.Instance.LevelTable.Level(this.exp); }
        }

        /// <summary>
        /// 指定したパラメータの実際値を取得する
        /// 基礎パラメータ + レベルアップの増加 + 餌付けの増加
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public int GetParam(Param param)
        {
            var res = Familiar.baseParam[(int)param];                   // 基礎パラメータ
            res += Familiar.additionParam[(int)param] * (Level - 1);    // レベルアップによる増加パラメータ
            res += this.param[(int)param];                              // 餌付けによる増加パラメータ
            return res;
        }
    }
}

