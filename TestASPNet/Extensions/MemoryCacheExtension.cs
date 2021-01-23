using Microsoft.Extensions.Caching.Memory;

namespace TestASPNet.Extensions
{
    public static class MemoryCacheExtension
    {
        /// <summary>
        ///  Generic method for get value from memory-cache
        /// </summary>
        /// <typeparam name="TIn">Generic input type</typeparam>
        /// <typeparam name="TOut">Generic out type</typeparam>
        /// <param name="cache">MemoryCache interface</param>
        /// <param name="key">key for searching</param>
        public static TOut Get<TIn, TOut>(this IMemoryCache cache, TIn key) where TOut : class
            => cache.Get<TOut>(key);
    }
}