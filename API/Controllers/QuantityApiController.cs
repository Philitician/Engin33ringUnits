using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QuantityTypeGrpcService;

namespace API.Controllers
{
    [ApiController]
    [Route("api/quantity")]
    public class QuantityApiController : ControllerBase
    {
        private readonly QuantityType.QuantityTypeClient _quantityTypeClient;
        private readonly ILogger<QuantityApiController> _logger;

        public QuantityApiController(QuantityType.QuantityTypeClient quantityTypeClient, ILogger<QuantityApiController> logger)
        {
            _quantityTypeClient = quantityTypeClient;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<string>> GetAll()
        {
            var quantityTypes = await _quantityTypeClient.GetAllAsync(new Empty());
            return quantityTypes.Notation;
        }

        [HttpGet("{notation}")]
        public async Task<IActionResult> GetUnitByQuantityType(string notation)
        {
            try
            {
                var units = await _quantityTypeClient
                    .GetUnitsByQuantityTypeAsync(new QuantityTypeNotation {Notation = notation});

                return Ok(units.Units_);
            }
            catch (RpcException e)
            {
                _logger.LogError(e.Message);
                return NotFound(e.Status.Detail);
            }
        }
    }
}