using EngineeringUnitsCore.Data;
using EngineeringUnitsCore.Data.Entities;
using EngineeringUnitsCore.DLA.AccessorContracts;
using EngineeringUnitsCore.DLA.Common;

namespace EngineeringUnitsCore.DLA.Accessors
{
    public class QuantityAccessor : AccessorBase<QuantityType>, IQuantityAccessor
    {
        public QuantityAccessor(EngineeringUnitsContext context) : base(context) { }
    }
}