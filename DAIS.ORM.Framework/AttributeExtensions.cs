﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DAIS.ORM.Framework
{
    internal class AttributeValues
    {
        public string PropertyName { get; set; }
        public string AttributeName { get; set; }
        public object Value { get; set; }
        public Type ValueType { get; set; }
        public ColumnType Type { get; set; }

        public string ParameterName => "@" + PropertyName;


        private string TypeString()
        {
            switch (Type)
            {
                case ColumnType.Normal:
                    return "";
                case ColumnType.PrimaryKey:
                    return "PK";
                case ColumnType.ForeignKey:
                    return "FK";
            }
            return string.Empty;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(PropertyName);
            if (Type != ColumnType.Normal)
                sb.AppendFormat("({0})", TypeString());
            sb.AppendFormat("({0})={1}", AttributeName, Value);

            return sb.ToString();
        }
    }

    internal static class AttributeExtensions
    {
        internal static string GetTableName(this Type type)
        {
            var tableNameAttribute = type.GetCustomAttributes(false)
                .OfType<TableNameAttribute>();

            if (tableNameAttribute == null)
                throw new ApplicationException("TableNameAttribute is missing.");

            return tableNameAttribute.FirstOrDefault().Name;
        }

        internal static IEnumerable<AttributeValues> GetAttributeNameValues<T>(this T @object)
        {
            var properties = @object
                .GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var pi in properties)
            {
                var columnAttribute = pi.GetCustomAttribute<ColumnAttribute>();

                if (columnAttribute != null)
                    yield return new AttributeValues
                    {
                        AttributeName = columnAttribute.Name,
                        PropertyName = pi.Name,
                        Value = pi.GetValue(@object),
                        ValueType = pi.PropertyType.IsNullable() ? Nullable.GetUnderlyingType(pi.PropertyType) : pi.PropertyType,
                        Type = columnAttribute.Type,
                    };
            }
        }

        internal static bool IsNullable(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        internal static string ToSqlNameParams(this IEnumerable<AttributeValues> collection)
        {
            return string.Join(", ", collection.Select(a => $"[{a.AttributeName}]"));
        }

        internal static string ToSqlParamNames(this IEnumerable<AttributeValues> collection)
        {
            return string.Join(", ", collection.Select(a => a.ParameterName));
        }
    }
}
