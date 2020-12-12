using System.Linq;

namespace EngineeringUnitsCore.Data.DbInitializer
{
    public static class Initializer
    {
        private const string Path = "Data/poscUnits22.xml";

        public static void TryInitialize(EngineeringUnitsContext context)
        {
            /*if (context.UnitOfMeasures.Any())
                return;*/
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var units = XmlHandler.GetUnits(Path);
            context.UnitOfMeasures.AddRange(units);
            context.SaveChanges();
        }
    }
}