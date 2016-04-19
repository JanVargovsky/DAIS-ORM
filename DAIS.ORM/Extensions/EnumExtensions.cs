using System;
using System.Linq;

namespace DAIS.ORM.Extensions
{
    [AttributeUsage(AttributeTargets.Enum)]
    public class NameAttribute : Attribute
    {
        public string Name { get; }

        public NameAttribute(string name)
        {
            Name = name;
        }
    }

    public static class EnumExtensions
    {
        public static string Name<T>(this T t) where T : struct
        {
            var type = t.GetType();

            var nameAttribute = (NameAttribute)(type
                .GetMember(t.ToString())
                .First()
                .GetCustomAttributes(typeof(NameAttribute), false)[0]);

            return nameAttribute?.Name;
        }
    }
}
