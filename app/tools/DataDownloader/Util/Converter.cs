using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util
{
    class Converter
    {
        public static void RegisterArray<T>()
        {
            TypeDescriptor.AddProvider(new ArrayTypeDescriptionProvider<T>(), typeof(T[]));
        }

        public static T ConvertFromString<T>(string value)
        {
            var converter = TypeDescriptor.GetConverter(typeof(T));
            return (T)converter.ConvertFromString(value);
        }

        /// <summary>
        /// 配列変換
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private class ArrayTypeDescriptionProvider<T> : TypeDescriptionProvider
        {
            public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
            {
                if (objectType.Name == typeof(T[]).Name)
                {
                    return new ArrayDescriptor<T>();
                }
                return base.GetTypeDescriptor(objectType, instance);
            }
        }

        private class ArrayDescriptor<T> : CustomTypeDescriptor
        {

            public override TypeConverter GetConverter()
            {
                return new TArrayConverter<T>();
            }
        }

        private class TArrayConverter<T> : ArrayConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                if (sourceType == typeof(T))
                {
                    return true;
                }
                return base.CanConvertFrom(context, sourceType);
            }
            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                var str = value as string;

                if (!string.IsNullOrEmpty(str))
                {
                    return str.Split(',').Select(s => ConvertFromString<T>(s)).ToArray();
                }
                return base.ConvertFrom(context, culture, value);
            }
        }

    }
}
