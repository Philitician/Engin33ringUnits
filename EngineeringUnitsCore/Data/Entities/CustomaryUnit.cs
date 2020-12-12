using System.Collections.Generic;

namespace EngineeringUnitsCore.Data.Entities
{
    public class CustomaryUnit : UnitOfMeasure
    {
        public int ConversionToBaseUnitId { get; set; }
        public ConversionToBaseUnit ConversionToBaseUnit { get; set; }

        public CustomaryUnit() : base() { }

        public CustomaryUnit(string id, 
            string annotation, 
            string name, 
            DimensionalClass dimensionalClass, 
            ICollection<QuantityType> quantityTypes,
            ConversionToBaseUnit conversionToBaseUnit) 
            : base(id, annotation, name, dimensionalClass, quantityTypes)
        {
            ConversionToBaseUnit = conversionToBaseUnit;
        }
    }
}