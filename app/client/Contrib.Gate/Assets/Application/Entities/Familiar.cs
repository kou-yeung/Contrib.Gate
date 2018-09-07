///============================
/// データ : 使い魔
///============================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CsvHelper;
using CsvHelper.TypeConversion;
using CsvHelper.Configuration;

namespace Entities
{
    public enum Race
    {
        Beast,
        Undead,
        Fly,
        Insect,
        Plant,
        Amorphas,
        Metal,
        Dragon,
        Human,
        Other,
    }

    public class Familiar
    {
        public Identify identify;
        public string Name;
        public Race Race;
    }

    public sealed class FamiliarMap : ClassMap<Familiar>
    {
        public FamiliarMap()
        {
            Map(x => x.identify).Index(0).TypeConverter<IdentifyTypeConverter>();
            Map(x => x.Name).Index(1);
            Map(x => x.Race).Index(2).TypeConverter<Util.EnumTypeConverter<Race>>();
        }
    }
}
