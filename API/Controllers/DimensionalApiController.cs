using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DimensionalClassGrpcService;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/dimensional")]
    public class DimensionalApiController
    {
        private readonly DimensionalClass.DimensionalClassClient _dimensionalClassClient;

        public DimensionalApiController(DimensionalClass.DimensionalClassClient dimensionalClassClient)
        {
            _dimensionalClassClient = dimensionalClassClient;
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
            Units units = await _dimensionalClassClient.GetUnitsByDimensionalClassAsync(new DimensionalClassNotation {Notation = notation});
            return units.Units_;
        }
    }
}