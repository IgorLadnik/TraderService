using System;
using System.Collections.Generic;
using System.Security.Claims;
using GraphQL.Execution;
//using AuthRolesLib;

namespace GraphQlHelperLib
{
    public interface IGqlCache 
    {
        object Value { get; set; }
    }

    public static class ProvideUserContextEx 
    {
        private static IDictionary<string, object> GetCacheDictionary(this IProvideUserContext context) => context.UserContext;

        public static bool DoesCacheExist(this IProvideUserContext context, string key) =>
            GetCacheDictionary(context).ContainsKey(key);

        
        // Cache

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


        // User for authentication

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

        public static bool GetIsAuthJwt(this IProvideUserContext context)
        {
            return GetCacheDictionary(context).TryGetValue("_IsAuthJwt", out object isAuthJwt)
                        ? (bool)isAuthJwt
                        : false;
        }

        public static void SetIsAuthJwt(this IProvideUserContext context, bool isAuthJwt)
        {
            GetCacheDictionary(context)["_IsAuthJwt"] = isAuthJwt;
        }

        //public static bool IsInRole(this IProvideUserContext context, params UserAuthRole[] userAuthTypes)
        //{
        //    var user = context.GetUser();
        //    //var claims = user?.Claims;
        //    if (user != null)
        //        foreach (var userAuthType in userAuthTypes)
        //            if (user.IsInRole($"{userAuthType}"))
        //                return true;

        //    return false;
        //}

        //public static void ValidateRole(this IProvideUserContext context, params UserAuthRole[] userAuthTypes)
        //{
        //   if (context.GetIsAuthJwt() && !context.IsInRole(userAuthTypes))
        //        throw new UnauthorizedAccessException();
        //}
    }
}
