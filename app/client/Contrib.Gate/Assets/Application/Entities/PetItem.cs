﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

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

        public int HP { get { return CalcParam(8, 2, 3, 3, 1); } }
        public int MP { get { return CalcParam(1, 2, 2, 2, 10); } }
        public int Attack { get { return CalcParam(.1f, 2, .2f, .2f, .1f); } }
        public int Defense { get { return CalcParam(.1f, .2f, 2f, .2f, .1f); } }
        public int Agility { get { return CalcParam(.1f, .2f, .2f, 2f, .1f); } }
        public int Magic { get { return CalcParam(-.3f, -.1f, .2f, -.1f, 3f); } }

        int CalcParam(params float[] p)
        {
            var res = .0f;
            for (int i = 0; i < (int)Score.Count; i++)
            {
                res += (Familiar.baseParam[i] + this.param[i]) * p[i];
            }
            return Mathf.FloorToInt(res);
        }

        public int GetParam(Param param)
        {
            switch (param)
            {
                case Param.HP: return HP;
                case Param.MP: return MP;
                case Param.Attack: return Attack;
                case Param.Defense: return Defense;
                case Param.Agility: return Agility;
                case Param.Magic: return Magic;
                default: return 0;
            }
        }

        /// <summary>
        /// 顔画像をロード
        /// </summary>
        /// <returns></returns>
        public Sprite GetFaceImage()
        {
            return Resources.LoadAll<Sprite>($"Familiar/{Familiar.Image}/face")[0];
        }
    }
}

