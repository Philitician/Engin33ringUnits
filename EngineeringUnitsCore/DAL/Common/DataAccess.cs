using EngineeringUnitsCore.Data;
using Microsoft.Extensions.Caching.Memory;

namespace EngineeringUnitsCore.DAL.Common
{
    public class DataAccess
    {
        private static IMemoryCache _cache;

        protected DataAccess(EngineeringUnitsContext context)
        {
            Context = context;
        }
        
        protected EngineeringUnitsContext Context { get; }
        
        protected static IMemoryCache Cache
            => _cache ??= new MemoryCache(new MemoryCacheOptions());
    }
}