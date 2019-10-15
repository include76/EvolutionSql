using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
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

        public static IEnumerable<T> ToEntities<T>(this IDataReader dataReader) where T : class, new()
        {
            var type = typeof(T);
            var properties = type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
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

        public static T ToEntity<T>(this IDataReader dataReader) where T : class, new()
        {
            var type = typeof(T);

            var entity = new T();
            // TODO: cache properties of entity
            var properties = type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            while (dataReader.Read())
            {
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
                break;
            }
            return entity;
        }

        private static void SetPropertyValue<T>(T entity, PropertyInfo property, object value)
        {
            if (value == DBNull.Value)
            {
                var defaultValue = GetDefaultValue(property.PropertyType);
                property.SetValue(entity, defaultValue);
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
