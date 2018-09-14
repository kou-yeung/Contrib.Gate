using CsvHelper.Configuration;
using Entities;
using Util;

public class CsvHelperRegister
{
    public static Configuration configuration { get; private set; }

    static CsvHelperRegister()
    {
        configuration = new Configuration();
        configuration.HeaderValidated = null;
        AddConverter();
        RegisterClassMap();
    }

    static void AddConverter()
    {
        configuration.TypeConverterCache.AddConverter<Identify>(new IdentifyTypeConverter());
        configuration.TypeConverterCache.AddConverter<Race>(new EnumTypeConverter<Race>());
    }

    static void RegisterClassMap()
    {
        configuration.RegisterClassMap<FamiliarMap>();
        configuration.RegisterClassMap<MaterialMap>();
        configuration.RegisterClassMap<VendingMap>();
        configuration.RegisterClassMap<RecipeMap>();
        configuration.RegisterClassMap<ItemMap>();
    }
}
