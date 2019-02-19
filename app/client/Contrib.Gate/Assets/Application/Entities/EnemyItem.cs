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

        public int GetParam(Param param)
        {
            List<int> addParam = new List<int>();
            foreach (var p in Enemy.Params) addParam.Add(p * Level);
            return Entity.Instance.CalcParam(param, Familiar.baseParam, addParam.ToArray());
        }
    }
}
