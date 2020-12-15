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
    public class UnitOfMeasureAccessor : AccessorBase<UnitOfMeasure>, IUnitOfMeasureAccessor
    {
        public UnitOfMeasureAccessor(EngineeringUnitsContext context) : base(context) { }

        public new async Task<UnitOfMeasure> Get(string id)
        {
            var key = "UnitOfMeasure_" + id;
            // returns unit if cached
            if (Cache.TryGetValue<UnitOfMeasure>(key, out var unit)) return unit;
            
            // gets unit from db if not cached
            unit = await Context
                .UnitOfMeasures
                .FirstOrDefaultAsync(u => u.Id == id);

            if (unit == null)
            {
                var unitByAlias = await GetByCondition(x => x.Name == id);
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