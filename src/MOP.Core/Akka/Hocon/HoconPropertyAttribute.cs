using System;
using System.Collections.Generic;
using System.Reflection;

namespace MOP.Core.Akka.Hocon
{
    [AttributeUsage(AttributeTargets.Property)]
    public class HoconPropertyAttribute : Attribute
    {
        public string Name { get; private set; }
        public HoconPropertyAttribute(string name)
        {
            Name = name;
        }

        public static IEnumerable<(string key, string value)> GetKeyValues(object source)
        {
            foreach (var prop in source.GetType().GetProperties())
            {
                foreach (var att in prop.GetCustomAttributes<HoconPropertyAttribute>())
                {
                    yield return ($"#!{att.Name}", Convert.ToString(prop.GetValue(source)));
                }
            }
        }
    }
}
