using System.Collections.Generic;
using System.Threading.Tasks;
using EngineeringUnitsCore.Data.Entities;

namespace EngineeringUnitsCore.DLA.AccessorContracts
{
    public interface IQuantityAccessor : IAccessorBase<QuantityType>
    {
        public Task<IEnumerable<UnitOfMeasure>> GetUnits(string qt);
        
        public Task<List<string>> GetQuantityTypes();
    }
}