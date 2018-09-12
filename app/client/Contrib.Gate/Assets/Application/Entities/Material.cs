using CsvHelper.Configuration;

namespace Entities
{
    public class Material
    {
        public Identify Identify;
        public string Name;
    }

    public sealed class MaterialMap : ClassMap<Material>
    {
        public MaterialMap()
        {
            Map(x => x.Identify).Index(0);
            Map(x => x.Name).Index(1);
        }
    }
}
