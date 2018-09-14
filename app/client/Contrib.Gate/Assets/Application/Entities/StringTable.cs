using CsvHelper.Configuration;
using System;
using System.Collections.Generic;

namespace Entities
{
    public class StringTable
    {
        Dictionary<string, string> strings = new Dictionary<string, string>();

        public StringTable(StringTableKV[] kv)
        {
            foreach (var item in kv)
            {
                strings.Add(item.Key, item.Value);
            }
        }

        public string Get(string key)
        {
            string res;
            strings.TryGetValue(key, out res);
            return res;
        }

        public string Get(ErrorCode code)
        {
            return Get(code.ToString());
        }
    }

    public class StringTableKV
    {
        public string Key;
        public string Value;
    }
    public sealed class StringTableMap : ClassMap<StringTableKV>
    {
        public StringTableMap()
        {
            Map(x => x.Key).Index(0);
            Map(x => x.Value).Index(1);
        }
    }
}
