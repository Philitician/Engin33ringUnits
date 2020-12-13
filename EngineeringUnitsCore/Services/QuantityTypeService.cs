using System.Linq;
using System.Threading.Tasks;
using EngineeringUnitsCore.DLA.AccessorContracts;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QuantityTypeGrpcService;

namespace EngineeringUnitsCore.Services
{
    public class QuantityTypeService : QuantityType.QuantityTypeBase
    {
        private readonly ILogger<QuantityTypeService> _logger;
        private readonly IQuantityAccessor _quantityAccessor;

        public QuantityTypeService(ILogger<QuantityTypeService> logger, IQuantityAccessor quantityAccessor)
        {
            _logger = logger;
            _quantityAccessor = quantityAccessor;
        }

        public override async Task<QuantityTypes> GetAll(Empty request, ServerCallContext context)
        {
            var qtList = await _quantityAccessor.GetAll();
            var quantityTypes =  new QuantityTypes();
            quantityTypes.Notation.AddRange(qtList.Select(x => x.Notation));
            return quantityTypes;
        }

        public override async Task<Units> GetUnitsByQuantityType(QuantityTypeNotation request, ServerCallContext context)
        {
            var qt = await _quantityAccessor.GetUnits(request.Notation);
            
            var units = new Units();
            
            units.Units_.AddRange(qt.Select(x => 
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