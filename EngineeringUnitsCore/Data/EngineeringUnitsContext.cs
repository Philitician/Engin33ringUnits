using EngineeringUnitsCore.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace EngineeringUnitsCore.Data
{
    public class EngineeringUnitsContext : DbContext
    {
        public EngineeringUnitsContext(DbContextOptions options) : base(options) { }
        public DbSet<Entities.UnitOfMeasure> UnitOfMeasures { get; set; }
        public DbSet<Entities.CustomaryUnit> CustomaryUnits { get; set; }
        public DbSet<Entities.DimensionalClass> DimensionalClasses { get; set; }
        
        public DbSet<Entities.QuantityType> QuantityTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UnitOfMeasureQuantityType>().HasKey(uq =>
                new {uq.UnitOfMeasureId, uq.QuantityTypeId});

            modelBuilder.Entity<UnitOfMeasureQuantityType>()
                .HasOne(uq => uq.UnitOfMeasure)
                .WithMany(u => u.UnitOfMeasureQuantityTypes)
                .HasForeignKey(uq => uq.UnitOfMeasureId);

            modelBuilder.Entity<UnitOfMeasureQuantityType>()
                .HasOne(uq => uq.QuantityType)
                .WithMany(qt => qt.UnitOfMeasureQuantityTypes)
                .HasForeignKey(uq => uq.QuantityTypeId);
        }
    }
}