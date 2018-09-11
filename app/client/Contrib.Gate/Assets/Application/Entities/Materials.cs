using CsvHelper.Configuration;

namespace Entities
{
    public class Materials
    {
        public Identify Identify;
        public string Name;
    }

    public sealed class MaterialsMap : ClassMap<Materials>
    {
        public MaterialsMap()
        {
            Map(x => x.Identify).Index(0);
            Map(x => x.Name).Index(1);
        }
    }
}
