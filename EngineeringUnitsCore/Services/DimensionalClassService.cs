using System.Linq;
using System.Threading.Tasks;
using DimensionalClassGrpcService;
using EngineeringUnitsCore.DAL.AccessorContracts;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EngineeringUnitsCore.Services
{
    public class DimensionalClassService : DimensionalClass.DimensionalClassBase
    {
        private readonly ILogger<DimensionalClassService> _logger;
        private readonly IDimensionalAccessor _dimensionalAccessor;


        public DimensionalClassService(ILogger<DimensionalClassService> logger, IDimensionalAccessor dimensionalAccessor)
        {
            _logger = logger;
            _dimensionalAccessor = dimensionalAccessor;
        }
        
        public override async Task<DimensionalClasses> GetAll(Empty request, ServerCallContext context)
        {
            var dClassList = await _dimensionalAccessor.GetAll();
            var dClasses = new DimensionalClasses();
            dClasses.Notation.AddRange(dClassList.Select(x => x.Notation));
            return dClasses;
        }

        public override async Task<Units> GetUnitsByDimensionalClass(DimensionalClassNotation request, ServerCallContext context)
        {
            var dClass = await _dimensionalAccessor
                .Get(request.Notation,
                    x => x.Notation == request.Notation,
                    r => r.Include(
                        x => x.Units));

            
            var units = new Units();
            
            units.Units_.AddRange(dClass.First().Units.Select(x => 
                new Unit
                {
                    Id = x.Id,
                    Name = x.Name
                })
            );

            return units;
        }
    }
}