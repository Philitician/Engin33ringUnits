using EngineeringUnitsCore.DAL.AccessorContracts;
using EngineeringUnitsCore.DAL.Common;
using EngineeringUnitsCore.Data;
using EngineeringUnitsCore.Data.Entities;

namespace EngineeringUnitsCore.DAL.Accessors
{
    public class QuantityAccessor : AccessorBase<QuantityType>, IQuantityAccessor
    {
        public QuantityAccessor(EngineeringUnitsContext context) : base(context) { }
    }
}