///============================
/// データ : 使い魔
///============================
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
        public Identify Identify;
        public string Name;
        public Race Race;
    }

    public sealed class FamiliarMap : ClassMap<Familiar>
    {
        public FamiliarMap()
        {
            Map(x => x.Identify).Index(0);
            Map(x => x.Name).Index(1);
            Map(x => x.Race).Index(2);
        }
    }
}
