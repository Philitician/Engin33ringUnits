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
            // returns unit if cached
            if (Cache.TryGetValue<UnitOfMeasure>(id, out var unit)) return unit;
            
            // gets unit from db if not cached
            unit = await Context
                .CustomaryUnits
                .Include(u => u.ConversionToBaseUnit)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (unit == null)
            {
                var unitByAlias = await GetByCondition(x => x.Name == id);
                unit = unitByAlias.FirstOrDefault();
            }

            var entryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(1));
            
            // adds unit to cache
            Cache.Set(id, unit, entryOptions);

            return unit;
        }
    }
}