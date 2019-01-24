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
                strings.Add($"{item.MainKey}{item.SubKey}", item.Value);
            }
        }

        public string Get(string main, string sub)
        {
            string res;
            strings.TryGetValue($"{main}{sub}", out res);
            return res;
        }

        public string Get<T>(T obj)
        {
            if (obj is string)
            {
                return Get(obj as string, "");
            }
            else
            {
                return Get(typeof(T).Name, obj.ToString());
            }
        }
    }

    public class StringTableKV
    {
        public string MainKey;
        public string SubKey;
        public string Value;
    }
    public sealed class StringTableMap : ClassMap<StringTableKV>
    {
        public StringTableMap()
        {
            Map(x => x.MainKey).Index(0);
            Map(x => x.SubKey).Index(1);
            Map(x => x.Value).Index(2);
        }
    }
}
