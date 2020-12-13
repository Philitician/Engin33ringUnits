using System;
using System.Threading.Tasks;
using EngineeringUnitsCore.Data;
using EngineeringUnitsCore.DLA.AccessorContracts;
using EngineeringUnitsCore.DLA.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace EngineeringUnitsCore.DLA.Accessors
{
    public class DimensionalAccessor : AccessorBase<Data.Entities.DimensionalClass>, IDimensionalAccessor
    {
        public DimensionalAccessor(EngineeringUnitsContext context) : base(context) { }
        
        /*public new async Task<Data.Entities.DimensionalClass> Get(string id)
        {
            
            if (Cache.TryGetValue<Data.Entities.DimensionalClass>(id, out var dClass)) return dClass;
            
            dClass = await Context
                .DimensionalClasses
                .Include(dc => dc.Units)
                .FirstOrDefaultAsync(dc => dc.Notation == id);
            
            var entryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(1));
            
            // adds unit to cache
            Cache.Set(id, dClass, entryOptions);

            return dClass;
        }*/
    }
}