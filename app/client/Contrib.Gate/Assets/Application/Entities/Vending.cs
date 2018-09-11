using CsvHelper.Configuration;

namespace Entities
{
    public class Vending
    {
        public Identify Identify;
        public int Level;
        public int Price;
    }

    public sealed class VendingMap : ClassMap<Vending>
    {
        public VendingMap()
        {
            Map(x => x.Identify).Index(0);
            Map(x => x.Level).Index(1);
            Map(x => x.Price).Index(2);
        }
    }
}
