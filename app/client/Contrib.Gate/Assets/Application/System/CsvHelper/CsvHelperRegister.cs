using Entities;
using UnityEngine;

namespace CsvHelper
{
    public class CsvHelperRegister
    {
        public static Configuration.Configuration configuration { get; private set; }

        static CsvHelperRegister()
        {
            configuration = new Configuration.Configuration();
            configuration.HeaderValidated = null;
            AddConverter();
            RegisterClassMap();
        }

        static void AddConverter()
        {
            configuration.TypeConverterCache.AddConverter<Identify>(new IdentifyTypeConverter());
            configuration.TypeConverterCache.AddConverter<Race>(new EnumTypeConverter<Race>());
            configuration.TypeConverterCache.AddConverter<Vector2Int>(new Vector2IntTypeConverter());
        }

        static void RegisterClassMap()
        {
            configuration.RegisterClassMap<FamiliarMap>();
            configuration.RegisterClassMap<MaterialMap>();
            configuration.RegisterClassMap<VendingMap>();
            configuration.RegisterClassMap<RecipeMap>();
            configuration.RegisterClassMap<ItemMap>();
            configuration.RegisterClassMap<CheatMap>();
            configuration.RegisterClassMap<StringTableMap>();
            configuration.RegisterClassMap<DungeonMap>();
            configuration.RegisterClassMap<RoomMap>();
            configuration.RegisterClassMap<StageMap>();
            configuration.RegisterClassMap<LevelMap>();
        }
    }
}
