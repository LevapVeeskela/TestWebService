using System;

namespace TestASPNet.Extensions
{
    /// <summary>
    /// Is it my implementation pattern QueryBuilder
    /// </summary>
    public static class QueryBuilder
    {
        public static string Query<T>(this string template, params T[] values) where T : class
            => string.Format(template, values);

    }
}