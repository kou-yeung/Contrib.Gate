using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CsvHelper;
using System.IO;
using System.Linq;
using System;

namespace Entities
{
    public class Entity
    {
        static Entity instance;
        public static Entity Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Entity();
                    instance.Load();
                }
                return instance;
            }
        }

        public Familiar[] Familiars { get; private set; }
        public Materials[] Materials { get; private set; }
        public Vending[] Vendings { get; private set; }

        void Load()
        {
            Familiars = Parse<Familiar>("Entities/familiar");
            Materials = Parse<Materials>("Entities/materials");
            Vendings = Parse<Vending>("Entities/vending");
        }

        public string Name(Identify identify)
        {
            switch (identify.Type)
            {
                case IDType.Materials:
                    return Array.Find(Materials, (v) =>
                    {
                        var a = identify;
                        return v.Identify == identify;
                    }).Name;
                case IDType.Familiar:
                    return Array.Find(Familiars, (v) => v.Identify == identify).Name;
            }
            return "";
        }

        T[] Parse<T>(string fn)
        {
            var str = Resources.Load<TextAsset>(fn).text.Trim();
            using (var csv = new CsvReader(new StringReader(str), CsvHelperRegister.configuration))
            {
                return csv.GetRecords<T>().ToArray();
            }
        }
    }
}
