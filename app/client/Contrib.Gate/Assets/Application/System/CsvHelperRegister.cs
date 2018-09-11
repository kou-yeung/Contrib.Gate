using UnityEngine;
using CsvHelper.TypeConversion;
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
        configuration.TypeConverterCache.AddConverter<Entities.Identify>(new IdentifyTypeConverter());
        configuration.TypeConverterCache.AddConverter<Entities.Race>(new EnumTypeConverter<Race>());
    }

    static void RegisterClassMap()
    {
        configuration.RegisterClassMap<FamiliarMap>();
        configuration.RegisterClassMap<MaterialsMap>();
    }
}
