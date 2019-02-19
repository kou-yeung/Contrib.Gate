using System.Collections;
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

        public int GetParam(Param param)
        {
            return Entity.Instance.CalcParam(param, Familiar.baseParam, this.param);
        }

        /// <summary>
        /// 顔画像をロード
        /// </summary>
        /// <returns></returns>
        public Sprite GetFaceImage()
        {
            return Resources.LoadAll<Sprite>($"Familiar/{Familiar.Image}/face")[0];
        }

        public int CalcParam(Param param, int[] addParam)
        {
            return 0;
        }
    }
}

