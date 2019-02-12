using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Entities
{
    public partial class EnemyItem
    {
        private Enemy enemy;
        public Enemy Enemy
        {
            get
            {
                if (enemy == null)
                {
                    enemy = Array.Find(Entity.Instance.Enemies, v => v.Identify == this.id);
                }
                return enemy;
            }
        }

        private Familiar familiar;
        public Familiar Familiar
        {
            get
            {
                if (familiar == null)
                {
                    familiar = Array.Find(Entity.Instance.Familiars, v => v.Identify == this.Enemy.FamiliarID);
                }
                return familiar;
            }
        }

        /// <summary>
        /// レベル取得
        /// </summary>
        public int Level
        {
            get { return this.level; }
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
            return res;
        }
    }
}

