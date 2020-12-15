using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DimensionalClassGrpcService;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using ConversionGrpcService;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [ApiController]
    [Route("api/dimensional")]
    public class DimensionalApiController
    {
        private readonly DimensionalClass.DimensionalClassClient _dimensionalClassClient;
        private readonly ILogger<DimensionalApiController> _logger;

        public DimensionalApiController(DimensionalClass.DimensionalClassClient dimensionalClassClient, ILogger<DimensionalApiController> logger)
        {
            _dimensionalClassClient = dimensionalClassClient;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<string>> GetAll()
        {
            var dClasses = await _dimensionalClassClient.GetAllAsync(new Empty());
            return dClasses.Notation;
        }

        [HttpGet("{*notation}")]
        public async Task<IEnumerable<Unit>> GetUnitsByDimensionalClass(string notation)
        {
            try
            {
                Units units = await _dimensionalClassClient.GetUnitsByDimensionalClassAsync(new DimensionalClassNotation {Notation = notation});
                return units.Units_;
            }
            catch (RpcException e)
            {
                Console.WriteLine(e.Message);
                _logger.LogError(e.Message);
                throw;
            }
        }
    }
}