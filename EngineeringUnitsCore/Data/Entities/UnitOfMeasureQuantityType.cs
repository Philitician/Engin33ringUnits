namespace EngineeringUnitsCore.Data.Entities
{
    public class UnitOfMeasureQuantityType
    {
        public UnitOfMeasureQuantityType() { }

        public string UnitOfMeasureId { get; set; }
        public UnitOfMeasure UnitOfMeasure { get; set; }

        public string QuantityTypeId { get; set; }
        public QuantityType QuantityType { get; set; }
    }
}