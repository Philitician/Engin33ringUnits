using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using QuantityTypeGrpcService;

namespace API.Controllers
{
    [ApiController]
    [Route("api/quantity")]
    public class QuantityApiController
    {
        private readonly QuantityType.QuantityTypeClient _quantityTypeClient;

        public QuantityApiController(QuantityType.QuantityTypeClient quantityTypeClient)
        {
            _quantityTypeClient = quantityTypeClient;
        }

        [HttpGet]
        public async Task<IEnumerable<string>> GetAll()
        {
            var quantityTypes = await _quantityTypeClient.GetAllAsync(new Empty());
            return quantityTypes.Notation;
        }

        [HttpGet("{notation}")]
        public async Task<IEnumerable<Unit>> GetUnitByQuantityType(string notation)
        {
            var units = await _quantityTypeClient
                .GetUnitsByQuantityTypeAsync(new QuantityTypeNotation
                    {Notation = notation});

            return units.Units_;
        }
    }
}