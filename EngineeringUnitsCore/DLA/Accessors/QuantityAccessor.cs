using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EngineeringUnitsCore.Data;
using EngineeringUnitsCore.Data.Entities;
using EngineeringUnitsCore.DLA.AccessorContracts;
using EngineeringUnitsCore.DLA.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace EngineeringUnitsCore.DLA.Accessors
{
    public class QuantityAccessor : AccessorBase<QuantityType>, IQuantityAccessor
    {
        public QuantityAccessor(EngineeringUnitsContext context) : base(context) { }

        public async Task<List<string>> GetQuantityTypes()
        {
            var qts = await GetAll();
            return qts.Select(qt => qt.Notation).ToList();
        }
        public async Task<IEnumerable<UnitOfMeasure>> GetUnits(string qt)
        {
            var qTypes = await Get(qt);
            return qTypes.UnitOfMeasureQuantityTypes.Select(q => q.UnitOfMeasure);
        }

        public new async Task<QuantityType> Get(string id)
        {
            if (Cache.TryGetValue<QuantityType>(id, out var qt)) return qt;
            qt = await Context
                .QuantityTypes
                .Include(q => q.UnitOfMeasureQuantityTypes)
                .ThenInclude(x => x.UnitOfMeasure)
                .FirstOrDefaultAsync(q => q.Notation == id);
            
            var entryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(1));
            
            // adds unit to cache
            Cache.Set(id, qt, entryOptions);

            return qt;
        }
    }
}