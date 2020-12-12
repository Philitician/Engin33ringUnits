using System;
using System.Collections.Generic;

namespace EngineeringUnitsCore.Data.Entities
{
    public class UnitOfMeasure
    {
        public UnitOfMeasure() { }

        public UnitOfMeasure(
            string id, 
            string annotation, 
            string name, 
            DimensionalClass dimensionalClass, 
            ICollection<QuantityType> quantityTypes)
        {
            Id = id;
            Annotation = annotation;
            Name = name;

            try
            {
                dimensionalClass.Units.Add(this);
                DimensionalClassId = dimensionalClass.Notation;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            DimensionalClass = dimensionalClass;
            
            UnitOfMeasureQuantityTypes = new List<UnitOfMeasureQuantityType>();
            foreach (var qt in quantityTypes)
            {
                var jObject = new UnitOfMeasureQuantityType
                {
                    UnitOfMeasureId = Id,
                    UnitOfMeasure = this,
                    QuantityTypeId = qt.Notation,
                    QuantityType = qt
                };
                UnitOfMeasureQuantityTypes.Add(jObject);
                qt.UnitOfMeasureQuantityTypes.Add(jObject);
            }
        }

        public string Id { get; set; }
        public string Annotation { get; set; }
        public string Name { get; set; }
        
        public string DimensionalClassId { get; set; }
        public DimensionalClass DimensionalClass { get; set; }
        public ICollection<UnitOfMeasureQuantityType> UnitOfMeasureQuantityTypes { get; set; }
    }
}