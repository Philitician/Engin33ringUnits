using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace EngineeringUnitsCore.Data.Entities
{
    public class QuantityType
    {
        public QuantityType() { }

        public QuantityType(string notation)
        {
            Notation = notation;
            UnitOfMeasureQuantityTypes = new Collection<UnitOfMeasureQuantityType>();
        }

        [Key]
        public string Notation { get; set; }
        public ICollection<UnitOfMeasureQuantityType> UnitOfMeasureQuantityTypes { get; set; }
    }
}