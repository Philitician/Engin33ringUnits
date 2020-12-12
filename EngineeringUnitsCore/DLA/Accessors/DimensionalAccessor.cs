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
    public class DimensionalAccessor : AccessorBase<DimensionalClass>, IDimensionalAccessor
    {
        public DimensionalAccessor(EngineeringUnitsContext context) : base(context) { }


        public async Task<List<string>> GetDimensionalClasses()
        {
            var dClasses = await GetAll();
            return dClasses.Select(dc => dc.Notation).ToList();
        }
        
        public async Task<List<string>> GetUnits(string dClass)
        {
            var dimensionalClass = await Get(dClass);
            return dimensionalClass.Units.Select(unit => unit.Id).ToList();
        }

        public new async Task<DimensionalClass> Get(string id)
        {
            if (Cache.TryGetValue<DimensionalClass>(id, out var dClass)) return dClass;
            
            dClass = await Context
                .DimensionalClasses
                .Include(dc => dc.Units)
                .FirstOrDefaultAsync(dc => dc.Notation == id);
            
            var entryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(1));
            
            // adds unit to cache
            Cache.Set(id, dClass, entryOptions);

            return dClass;

        }
    }
}