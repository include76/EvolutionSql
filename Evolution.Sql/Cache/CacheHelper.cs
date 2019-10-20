using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;
using System.Text;

namespace Evolution.Sql.Cache
{
    internal sealed class CacheHelper
    {
        /// <summary>
        /// to cache stored procedure parameters get from INFORMATION_SCHEMA.PARAMETERS
        /// </summary>
        internal static ConcurrentDictionary<string, List<DbParameterCacheItem>> StoredProcedureParameterCache = new ConcurrentDictionary<string, List<DbParameterCacheItem>>();
        /// <summary>
        /// to cache properties of Types, so that no need to reflect to get properties repeatedlly
        /// </summary>
        internal static ConcurrentDictionary<string, PropertyInfo[]> TypePropertyCache = new ConcurrentDictionary<string, PropertyInfo[]>();

        internal static List<DbParameterCacheItem> GetDbParameters(string key)
        {
            try
            {
                if (StoredProcedureParameterCache.TryGetValue(key, out List<DbParameterCacheItem> dbParamters))
                {
                    return dbParamters;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        internal static void AddDbParameters(string key, List<DbParameterCacheItem> dbParameters)
        {
            try
            {
                StoredProcedureParameterCache.TryAdd(key, dbParameters);
            }
            catch
            {

            }
        }

        internal static PropertyInfo[] GetTypePropertyInfos(string key)
        {
            try
            {
                if (TypePropertyCache.TryGetValue(key, out PropertyInfo[] propertyInfos))
                {
                    return propertyInfos;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        internal static void AddTypePropertyInfos(string key, PropertyInfo[] propertyInfos)
        {
            try
            {
                TypePropertyCache.TryAdd(key, propertyInfos);
            }
            catch
            {

            }
        }
    }
}
