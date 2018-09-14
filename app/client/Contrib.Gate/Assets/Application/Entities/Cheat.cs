using CsvHelper.Configuration;
using System.Collections.Generic;

namespace Entities
{
    public class Cheat
    {
        public string Name;         // 表示名
        public string Command;      // コマンド名
        public List<string> Params; // パラメータ一覧
    }

    public sealed class CheatMap : ClassMap<Cheat>
    {
        public CheatMap()
        {
            Map(x => x.Name).Index(0);
            Map(x => x.Command).Index(1);
            Map(x => x.Params).ConvertUsing(row =>
            {
                var res = new List<string>();
                for (int i = 0; i < 5; i++)
                {
                    string param;
                    row.TryGetField(2 + i, out param);
                    if(!string.IsNullOrEmpty(param))
                    {
                        res.Add(param);
                    }
                }
                return res;
            });
        }
    }
}
