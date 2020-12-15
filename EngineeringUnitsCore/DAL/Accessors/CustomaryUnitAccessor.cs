using System;
using System.Linq;
using System.Threading.Tasks;
using EngineeringUnitsCore.DAL.AccessorContracts;
using EngineeringUnitsCore.DAL.Common;
using EngineeringUnitsCore.Data;
using EngineeringUnitsCore.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace EngineeringUnitsCore.DAL.Accessors
{
    public class CustomaryUnitAccessor : AccessorBase<CustomaryUnit>, ICustomaryUnitAccessor
    {
        public CustomaryUnitAccessor(EngineeringUnitsContext context) : base(context) { }

        public new async Task<CustomaryUnit> Get(string id)
        {
            var key = "CustomaryUnit_" + id;
            // returns unit if cached
            if (Cache.TryGetValue<CustomaryUnit>(key, out var unit)) return unit;
            
            // gets unit from db if not cached
            unit = await Context
                .CustomaryUnits
                .Include(u => u.ConversionToBaseUnit)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (unit == null)
            {
                var unitByAlias = await Get(
                    x => x.Name == id, 
                    r => r.Include(x => x.ConversionToBaseUnit));
                unit = unitByAlias.FirstOrDefault();
                if (unit == null)
                    return null;
            }

            var entryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(1));
            
            // adds unit to cache
            Cache.Set(key, unit, entryOptions);

            return unit;
        }
        
    }
}