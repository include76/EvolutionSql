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
    internal static class Extension
    {
        internal static void TryOpen(this IDbConnection dbConnection)
        {
            if (dbConnection.State != ConnectionState.Open)
            {
                dbConnection.Open();
            }
        }

        internal static IEnumerable<T> ToEntities<T>(this DbDataReader dataReader) where T : class, new()
        {
            if (dataReader == null || !dataReader.HasRows)
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
                    // if column contains underscore, make sure it can be map to C-Sharp property too
                    // for example: column first_name can map to property FirstName
                    var property = properties.FirstOrDefault(x => x.Name.Equals(columnName.Replace("_", ""), StringComparison.OrdinalIgnoreCase));
                    if (property != null)
                    {
                        //property.SetValue(entity, dataReader[i]);
                        SetPropertyValue<T>(entity, property, dataReader, i);
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

        /*
        private static void SetPropertyValue<T>(T obj, PropertyInfo property, DbDataReader dataReader, int index)
        {
            if (dataReader.GetValue(index) == DBNull.Value)
            {
                var defaultValue = GetDefaultValue(property.PropertyType);
                property.SetValue(obj, defaultValue);
                return;
            }

            if (property.PropertyType == typeof(int) || property.PropertyType == typeof(uint))
            {
                property.SetValue(obj, dataReader.GetInt32(index));
            }
            else if (property.PropertyType == typeof(long) || property.PropertyType == typeof(ulong))
            {
                property.SetValue(obj, dataReader.GetInt64(index));
            }
            else if (property.PropertyType == typeof(bool))
            {
                property.SetValue(obj, dataReader.GetBoolean(index));
            }
            else if (property.PropertyType == typeof(string))
            {
                property.SetValue(obj, dataReader.GetString(index));
            }
            else if (property.PropertyType == typeof(byte))
            {
                property.SetValue(obj, dataReader.GetByte(index));
            }
            else if (property.PropertyType == typeof(sbyte))
            {
                property.SetValue(obj, dataReader.GetByte(index));
            }
            else if (property.PropertyType == typeof(short))
            {
                property.SetValue(obj, dataReader.GetInt16(index));
            }
            else if (property.PropertyType == typeof(ushort))
            {
                property.SetValue(obj, dataReader.GetInt16(index));
            }
            else if (property.PropertyType == typeof(float))
            {
                property.SetValue(obj, dataReader.GetFloat(index));
            }
            else if (property.PropertyType == typeof(double))
            {
                property.SetValue(obj, dataReader.GetDouble(index));
            }
            else if (property.PropertyType == typeof(decimal))
            {
                property.SetValue(obj, dataReader.GetDecimal(index));
            }
            else if (property.PropertyType == typeof(char))
            {
                property.SetValue(obj, dataReader.GetChar(index));
            }
            else if (property.PropertyType == typeof(char[]))
            {
                //property.SetValue(obj, dataReader.GetFieldValue<char[]>(index));
                property.SetValue(obj, dataReader.GetString(index).ToCharArray());
            }
            else if (property.PropertyType == typeof(Guid))
            {
                property.SetValue(obj, dataReader.GetGuid(index));
            }
            else if (property.PropertyType == typeof(DateTime))
            {
                if (dataReader.GetFieldType(index) == typeof(TimeSpan))
                {
                    var datetime = new DateTime(dataReader.GetFieldValue<TimeSpan>(index).Ticks);
                    property.SetValue(obj, datetime);
                    return;
                }
                property.SetValue(obj, dataReader.GetDateTime(index));
            }
            else if (property.PropertyType == typeof(DateTimeOffset))
            {
                property.SetValue(obj, dataReader.GetDateTime(index));
            }
            else if (property.PropertyType == typeof(TimeSpan))
            {
                property.SetValue(obj, dataReader.GetDateTime(index));
            }
            else if (property.PropertyType == typeof(byte[]))
            {
                property.SetValue(obj, dataReader.GetFieldValue<byte[]>(index));
            }
            else if (property.PropertyType == typeof(byte?))
            {
                property.SetValue(obj, dataReader.GetFieldValue<byte?>(index));
            }
            else if (property.PropertyType == typeof(sbyte?))
            {
                property.SetValue(obj, dataReader.GetFieldValue<sbyte?>(index));
            }
            else if (property.PropertyType == typeof(short?))
            {
                property.SetValue(obj, dataReader.GetFieldValue<short?>(index));
            }
            else if (property.PropertyType == typeof(ushort?))
            {
                property.SetValue(obj, dataReader.GetFieldValue<ushort?>(index));
            }
            else if (property.PropertyType == typeof(int?))
            {
                property.SetValue(obj, dataReader.GetFieldValue<int?>(index));
            }
            else if (property.PropertyType == typeof(uint?))
            {
                property.SetValue(obj, dataReader.GetFieldValue<uint?>(index));
            }
            else if (property.PropertyType == typeof(long?))
            {
                property.SetValue(obj, dataReader.GetFieldValue<long?>(index));
            }
            else if (property.PropertyType == typeof(ulong?))
            {
                property.SetValue(obj, dataReader.GetFieldValue<ulong?>(index));
            }
            else if (property.PropertyType == typeof(float?))
            {
                property.SetValue(obj, dataReader.GetFieldValue<float?>(index));
            }
            else if (property.PropertyType == typeof(double?))
            {
                property.SetValue(obj, dataReader.GetFieldValue<double?>(index));
            }
            else if (property.PropertyType == typeof(decimal?))
            {
                property.SetValue(obj, dataReader.GetFieldValue<decimal?>(index));
            }
            else if (property.PropertyType == typeof(bool?))
            {
                property.SetValue(obj, dataReader.GetFieldValue<bool?>(index));
            }
            else if (property.PropertyType == typeof(char?))
            {
                property.SetValue(obj, dataReader.GetFieldValue<char?>(index));
            }
            else if (property.PropertyType == typeof(Guid?))
            {
                property.SetValue(obj, dataReader.GetFieldValue<Guid?>(index));
            }
            else if (property.PropertyType == typeof(DateTime?))
            {
                if(dataReader.GetFieldType(index) == typeof(TimeSpan))
                {
                    var datetime = new DateTime(dataReader.GetFieldValue<TimeSpan>(index).Ticks);
                    property.SetValue(obj, datetime);
                    return;
                }
                property.SetValue(obj, dataReader.GetFieldValue<DateTime?>(index));
            }
            else if (property.PropertyType == typeof(DateTimeOffset?))
            {
                property.SetValue(obj, dataReader.GetFieldValue<DateTimeOffset?>(index));
            }
            else if (property.PropertyType == typeof(TimeSpan?))
            {
                property.SetValue(obj, dataReader.GetFieldValue<TimeSpan?>(index));
            }
            else if (property.PropertyType == typeof(object))
            {
                property.SetValue(obj, dataReader.GetFieldValue<object>(index));
            }
            else
            {
                property.SetValue(obj, dataReader.GetValue(index));
            }
        }*/

        /// <summary>
        /// this does not work when
        /// db is char[] and property is char[], because dataReader[i] is string
        /// db is time and property is DateTime, because dataReader[i] is TimeSpan
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="property"></param>
        /// <param name="value"></param>
        private static void SetPropertyValue<T>(T obj, PropertyInfo property, DbDataReader dataReader, int index)
        {
            if (dataReader.GetValue(index) == DBNull.Value)
            {
                var defaultValue = GetDefaultValue(property.PropertyType);
                property.SetValue(obj, defaultValue);
                return;
            }
            if (property.PropertyType == typeof(char[]))
            {
                //property.SetValue(obj, dataReader.GetFieldValue<char[]>(index));
                property.SetValue(obj, dataReader.GetString(index).ToCharArray());
            }
            else if (property.PropertyType == typeof(DateTime))
            {
                if (dataReader.GetFieldType(index) == typeof(TimeSpan))
                {
                    var datetime = new DateTime(dataReader.GetFieldValue<TimeSpan>(index).Ticks);
                    property.SetValue(obj, datetime);
                    return;
                }
                property.SetValue(obj, dataReader.GetFieldValue<DateTime>(index));
            }
            else if (property.PropertyType == typeof(DateTime?))
            {
                if (dataReader.GetFieldType(index) == typeof(TimeSpan))
                {
                    var datetime = new DateTime(dataReader.GetFieldValue<TimeSpan>(index).Ticks);
                    property.SetValue(obj, datetime);
                    return;
                }
                property.SetValue(obj, dataReader.GetFieldValue<DateTime?>(index));
            }
            else if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                var genericType = Nullable.GetUnderlyingType(property.PropertyType);
                if (genericType != null)
                {
                    property.SetValue(obj, Convert.ChangeType(dataReader.GetValue(index), genericType), null);
                }
            }
            else
            {
                property.SetValue(obj, Convert.ChangeType(dataReader.GetValue(index), property.PropertyType));
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
