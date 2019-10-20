using Evolution.Sql.Cache;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Evolution.Sql
{
    public static class Extension
    {
        public static void TryOpen(this IDbConnection dbConnection)
        {
            if (dbConnection.State != ConnectionState.Open)
            {
                dbConnection.Open();
            }
        }

        public static IEnumerable<T> ToEntities<T>(this DbDataReader dataReader) where T : class, new()
        {
            if(dataReader == null || !dataReader.HasRows)
            {
                return null;
            }
            var type = typeof(T);
            var properties = CacheHelper.GetTypePropertyInfos(type.FullName);
            if (properties == null)
            {
                properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                // cache type PropertInfos
                if (properties != null)
                {
                    CacheHelper.AddTypePropertyInfos(type.FullName, properties);
                }
            }
            var list = new List<T>();
            while (dataReader.Read())
            {
                var entity = new T();
                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    var columnName = dataReader.GetName(i);
                    var property = properties.FirstOrDefault(x => x.Name.Equals(columnName, StringComparison.OrdinalIgnoreCase));
                    if (property != null)
                    {
                        //property.SetValue(entity, dataReader[i]);
                        SetPropertyValue<T>(entity, property, dataReader[i]);
                    }
                }
                list.Add(entity);
            }
            return list.AsEnumerable();
        }

        /// <summary>
        /// Determines whether the given type is anonymous or not.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><see langword="true"/> if type is anonymous, <see langword="false"/> otherwise</returns>
        public static bool IsAnonymousType(this Type type)
        {
            if (type == null)
            {
                return false;
            }

            return type.GetTypeInfo().IsGenericType
                   && (type.GetTypeInfo().Attributes & TypeAttributes.NotPublic) == TypeAttributes.NotPublic
                   && (type.Name.StartsWith("<>", StringComparison.OrdinalIgnoreCase) || type.Name.StartsWith("VB$", StringComparison.OrdinalIgnoreCase))
                   && (type.Name.Contains("AnonymousType") || type.Name.Contains("AnonType"))
                   && type.GetTypeInfo().GetCustomAttributes(typeof(CompilerGeneratedAttribute)).Any();
        }

        private static void SetPropertyValue<T>(T entity, PropertyInfo property, object value)
        {
            if (value == DBNull.Value)
            {
                var defaultValue = GetDefaultValue(property.PropertyType);
                property.SetValue(entity, defaultValue);
            }
            else if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                var genericType = Nullable.GetUnderlyingType(property.PropertyType);
                if (genericType != null)
                {
                    property.SetValue(entity, Convert.ChangeType(value, genericType), null);
                }
            }
            else
            {
                property.SetValue(entity, Convert.ChangeType(value, property.PropertyType));
            }
        }

        private static object GetDefaultValue(Type t)
        {
            if (t.IsValueType)
            {
                return Activator.CreateInstance(t);
            }

            return null;
        }
    }
}
