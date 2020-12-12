using EngineeringUnitsCore.Data;
using EngineeringUnitsCore.Data.Entities;
using EngineeringUnitsCore.DLA.AccessorContracts;
using EngineeringUnitsCore.DLA.Common;

namespace EngineeringUnitsCore.DLA.Accessors
{
    public class UnitOfMeasureAccessor : AccessorBase<UnitOfMeasure>, IUnitOfMeasureAccessor
    {
        public UnitOfMeasureAccessor(EngineeringUnitsContext context) : base(context) { }

    }
}