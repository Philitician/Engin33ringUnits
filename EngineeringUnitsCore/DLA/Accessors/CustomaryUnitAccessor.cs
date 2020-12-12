using System;
using System.Threading.Tasks;
using EngineeringUnitsCore.Data;
using EngineeringUnitsCore.Data.Entities;
using EngineeringUnitsCore.DLA.AccessorContracts;
using EngineeringUnitsCore.DLA.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace EngineeringUnitsCore.DLA.Accessors
{
    public class CustomaryUnitAccessor : AccessorBase<CustomaryUnit>, ICustomaryUnitAccessor
    {
        public CustomaryUnitAccessor(EngineeringUnitsContext context) : base(context) { }

        public new async Task<CustomaryUnit> Get(string id)
        {
            // returns unit if cached
            if (Cache.TryGetValue<CustomaryUnit>(id, out var unit)) return unit;
            
            // gets unit from db if not cached
            unit = await Context
                .CustomaryUnits
                .Include(u => u.ConversionToBaseUnit)
                .FirstOrDefaultAsync(u => u.Id == id);

            var entryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(1));
            
            // adds unit to cache
            Cache.Set(id, unit, entryOptions);

            return unit;
        }
        
    }
}