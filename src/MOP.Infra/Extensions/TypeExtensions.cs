using Optional;
using Optional.Unsafe;
using System;
using System.Reflection;

using static MOP.Infra.Optional.Static;

namespace MOP.Infra.Extensions
{
    public static class TypeExtensions
    {
        public static void SetValue<T>(this Type type, object target, string propName, T value)
        {
            var prop = type.GetPropertyIgnoreCase(propName);
            if (!prop.HasValue) 
                throw new ArgumentException("Can't find property with name {@propName}", propName);

            var pinfo = prop.ValueOrFailure();
            var convertValue = Convert.ChangeType(value, pinfo.PropertyType);
            pinfo.SetValue(target, convertValue);
        }

        public static Option<PropertyInfo> GetPropertyIgnoreCase(this Type type, string name)
        {
            foreach (var p in type.GetProperties())
            {
                if (p.Name.EqualIgnoreCase(name))
                    return Some(p);
            }
            return None<PropertyInfo>();
        }
    }
}
