using EngineeringUnitsCore.Data;
using EngineeringUnitsCore.DLA.AccessorContracts;
using EngineeringUnitsCore.DLA.Common;

namespace EngineeringUnitsCore.DLA.Accessors
{
    public class DimensionalAccessor : AccessorBase<Data.Entities.DimensionalClass>, IDimensionalAccessor
    {
        public DimensionalAccessor(EngineeringUnitsContext context) : base(context) { }
    }
}