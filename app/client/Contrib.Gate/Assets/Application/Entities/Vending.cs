using CsvHelper.Configuration;

namespace Entities
{
    public class Vending
    {
        public Identify Identify;
        public Identify Result;
        public int Level;
        public int Price;
        public int Num;
    }

    public sealed class VendingMap : ClassMap<Vending>
    {
        public VendingMap()
        {
            Map(x => x.Identify).Index(0);
            Map(x => x.Result).Index(1);
            Map(x => x.Level).Index(2);
            Map(x => x.Price).Index(3);
            Map(x => x.Num).Index(4);
        }
    }
}
