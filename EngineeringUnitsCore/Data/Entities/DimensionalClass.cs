using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EngineeringUnitsCore.Data.Entities
{
    public class DimensionalClass
    {
        public DimensionalClass() { }

        public DimensionalClass(string notation)
        {
            Notation = notation; 
            Units = new List<UnitOfMeasure>();
        }
        
        [Key]
        public string Notation { get; set; }

        public List<UnitOfMeasure> Units { get; set; }
    }
}