using CsvHelper.Configuration;


namespace Entities
{
    public class Item
    {
        public Identify Identify;
        public string Name;
    }

    public sealed class ItemMap : ClassMap<Item>
    {
        public ItemMap()
        {
            Map(x => x.Identify).Index(0);
            Map(x => x.Name).Index(1);
        }
    }
}
