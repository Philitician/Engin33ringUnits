namespace EngineeringUnitsCore.Data.Entities
{
    public class ConversionToBaseUnit
    {
        public ConversionToBaseUnit()
        {
            A = D = 0; 
            B = C = 1;
        }
        
        public ConversionToBaseUnit(string baseUnit, double b = 1, double c = 1, double a = 0, double d = 0)
        {
            BaseUnit = baseUnit;
            A = a;
            B = b;
            C = c;
            D = d;
        }

        public int Id { get; set; }
        
        public string BaseUnit { get; set; }

        public double A { get; set; }

        public double B { get; set; }

        public double C { get; set; }

        public double D { get; set; }
        
    }
}