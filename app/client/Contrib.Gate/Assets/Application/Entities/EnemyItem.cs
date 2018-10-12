using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Entities
{
    public partial class EnemyItem
    {
        private Enemy _enemy;
        public Enemy Enemy
        {
            get
            {
                if (_enemy == null)
                {
                    _enemy = Array.Find(Entity.Instance.Enemies, v => v.Identify == this.id);
                }
                return _enemy;
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
            var res = Enemy.baseParam[(int)param];                   // 基礎パラメータ
            res += Enemy.additionParam[(int)param] * (Level - 1);    // レベルアップによる増加パラメータ
            return res;
        }
    }
}

