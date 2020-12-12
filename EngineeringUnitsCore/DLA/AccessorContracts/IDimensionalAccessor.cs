using System.Collections.Generic;
using System.Threading.Tasks;
using EngineeringUnitsCore.Data.Entities;

namespace EngineeringUnitsCore.DLA.AccessorContracts
{
    public interface IDimensionalAccessor : IAccessorBase<DimensionalClass>
    {
        public Task<List<string>> GetUnits(string dClass);

        public Task<List<string>> GetDimensionalClasses();
    }
}