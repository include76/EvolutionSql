using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
            var properties = type.GetProperties(System.Reflection.BindingFlags.Public);
            var list = new List<T>();
            while (dataReader.Read())
            {
                var entity = new T();
                foreach (DataColumn column in dataReader.GetSchemaTable().Columns)
                {
                    var property = properties.FirstOrDefault(x => x.Name == column.ColumnName);
                    if (property != null)
                    {
                        property.SetValue(entity, dataReader[column.ColumnName]);
                    }
                }
                list.Add(entity);
            }
            return list.AsEnumerable();
        }

        public static T ToEntity<T>(this IDataReader dataReader) where T: class, new()
        {
            var type = typeof(T);

            var entity = new T();
            // TODO: cache properties of entity
            var properties = type.GetProperties(System.Reflection.BindingFlags.Public);
            while (dataReader.Read())
            {
                foreach (DataColumn column in dataReader.GetSchemaTable().Columns)
                {
                    var property = properties.FirstOrDefault(x => x.Name == column.ColumnName);
                    if (property != null)
                    {
                        property.SetValue(entity, dataReader[column.ColumnName]);
                    }
                }
                break;
            }
            return entity;
        }
    }
}
