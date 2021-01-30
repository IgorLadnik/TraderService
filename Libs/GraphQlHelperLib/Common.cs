using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using GraphQL;
using GraphQL.Execution;

namespace GraphQlHelperLib
{
    public interface IGqlCache 
    {
        object Value { get; set; }
    }

    public static class Extensions
    {
        private static IDictionary<string, object> GetCacheDictionary(this IProvideUserContext context) => context.UserContext;

        public static bool DoesCacheExist(this IProvideUserContext context, string key) =>
            GetCacheDictionary(context).ContainsKey(key);

        #region Cache

        public static T GetCache<T>(this IProvideUserContext context, string key)
        {
            if (!GetCacheDictionary(context).TryGetValue(key, out object cacheObj))
                return default;

            return (T)(cacheObj as IGqlCache).Value;
        }

        public static void SetCache<T>(this IProvideUserContext context, string key, object value) where T : IGqlCache, new()
        {
            if (value == null)
                return;

            T cacheObj = new();
            cacheObj.Value = value;
            GetCacheDictionary(context)[key] = cacheObj;
        }

        #endregion Cache

        #region User for authentication

        public static ClaimsPrincipal GetUser(this IProvideUserContext context)
        {
            return GetCacheDictionary(context).TryGetValue("_User", out object user)
                        ? (ClaimsPrincipal)user
                        : null;
        }

        public static void SetUser(this IProvideUserContext context, ClaimsPrincipal user)
        {
            if (user == null)
                return;

            GetCacheDictionary(context)["_User"] = user;
        }

        public static T GetArgument<T>(this IResolveFieldContext context, string argName)
        {
            var argNullable = context.Arguments[argName];
            return argNullable != null ? (T)argNullable : default;
        }

        public static Dictionary<string, object> GetArgument(this IResolveFieldContext context, string argName)
        {
            var argNullable = context.Arguments[argName];
            if (argNullable != null)
            {
                var dct = argNullable as Dictionary<string, object>;
                if (dct != null)
                    return dct;
            }

            return null;
        }

        #endregion // User for authentication

        #region Sort

        public static object GetPropValue(this object t, string propName) =>
            t.GetType().GetProperties().Where(p => p.Name.ToLower() == propName.ToLower()).FirstOrDefault().GetValue(t);

        public static List<T> Sort<T>(this List<T> lstT ,string sortBy) 
        {
            if (string.IsNullOrEmpty(sortBy))
                return lstT;

            return sortBy[0] == '!'
                    ? lstT.OrderByDescending(t => t.GetPropValue(sortBy.Substring(1))).ToList()
                    : lstT.OrderBy(t => t.GetPropValue(sortBy)).ToList();

        }

        #endregion // Sort

        #region Pagination

        public static List<T> Page<T>(this List<T> lstT, int pageSize, int currentPage)
        {
            if (pageSize == 0)
                return lstT;

            return lstT.Skip(currentPage * pageSize).Take(pageSize).ToList();
        }

        #endregion // Pagination
    }
}
