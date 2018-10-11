using CsvHelper.Configuration;
using System.Collections.Generic;

namespace Entities
{
    public class Cheat
    {
        public class Param
        {
            public string name;
            public string defaultValue;
        }
        public string Name;         // 表示名
        public string Exec;         // 実行方法
        public string Command;      // コマンド名
        public List<Param> Params;  // パラメータ一覧
    }

    public sealed class CheatMap : ClassMap<Cheat>
    {
        public CheatMap()
        {
            Map(x => x.Name).Index(0);
            Map(x => x.Exec).Index(1);
            Map(x => x.Command).Index(2);
            Map(x => x.Params).ConvertUsing(row =>
            {
                var res = new List<Cheat.Param>();
                var start = 3;
                var offset = 0;
                for (int i = 0; i < 5; i++)
                {
                    var param = new Cheat.Param();
                    row.TryGetField(start + (offset++), out param.name);
                    row.TryGetField(start + (offset++), out param.defaultValue);
                    if (string.IsNullOrEmpty(param.name)) continue;
                    if (string.IsNullOrEmpty(param.defaultValue)) param.defaultValue = "";
                    res.Add(param);
                }
                return res;
            });
        }
    }
}
