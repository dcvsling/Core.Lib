//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;

//namespace Core.Lib.Reflections
//{

//    public static class PropertyHelper
//    {
//        public static IEnumerable<PropertyInfo> GetDeepProperties(this Type type)
//            => type.GetProperties()
//                .Recurse<IEnumerable<PropertyInfo>, IEnumerable<PropertyInfo>>(next => ps => ps.Union(
//                    ps.SelectMany(p => p.IsAllowDeep()
//                        ? next(p.PropertyType.GetProperties())
//                        : Enumerable.Empty<PropertyInfo>())));

//        public static IEnumerable<TResult> GetDeepProperties<TResult>(this Type type, Func<IEnumerable<PropertyInfo>, PropertyInfo, TResult> selector)
//            => type.GetProperties()
//                .Recurse<IEnumerable<PropertyInfo>, IEnumerable<TResult>>(next => ps => ps.Select(x => selector(ps, x))
//                     .Union(ps.SelectMany(
//                         p => p.IsAllowDeep()
//                             ? next(p.PropertyType.GetProperties())
//                             : Enumerable.Empty<TResult>())));

//        private static bool IsAllowDeep(this PropertyInfo p)
//            => !p.PropertyType.IsValueTypeOrString() && p.PropertyType != p.DeclaringType && !p.GetMethod.IsStatic;

//        internal static string GetFullName(this PropertyInfo property)
//          => $"{property.DeclaringType.Namespace}.{property.DeclaringType.GetFormatName()}.{property.Name}";
//    }

//}
