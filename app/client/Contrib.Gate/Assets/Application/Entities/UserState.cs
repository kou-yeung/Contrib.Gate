using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Event;

namespace Entities
{
    public partial class UserState
    {
        public static readonly string Update = @"UserStat:Update";

        public void AddCoin(int added)
        {
            SetCoin(this.coin + added);
        }
        public void SetCoin(int coin)
        {
            this.coin = coin;
            Observer.Instance.Notify(Update, this);
        }
    }
}
