using EngineeringUnitsCore.DAL.AccessorContracts;
using EngineeringUnitsCore.DAL.Common;
using EngineeringUnitsCore.Data;

namespace EngineeringUnitsCore.DAL.Accessors
{
    public class DimensionalAccessor : AccessorBase<Data.Entities.DimensionalClass>, IDimensionalAccessor
    {
        public DimensionalAccessor(EngineeringUnitsContext context) : base(context) { }
    }
}