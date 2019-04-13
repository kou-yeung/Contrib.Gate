using CsvHelper.Configuration;

namespace Entities
{
    public class Configs
    {
        public string AppVersion;
    }

    public sealed class ConfigsMap : ClassMap<Configs>
    {
        public ConfigsMap()
        {
            Map(x => x.AppVersion).Index(0);
        }
    }
}
