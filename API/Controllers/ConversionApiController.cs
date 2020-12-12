using System;
using System.Threading.Tasks;
using EngineeringUnitsCore;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/conversion")]
    public class ConversionApiController : ControllerBase
    {
        private readonly Conversion.ConversionClient _conversionClient;
        
        public ConversionApiController(Conversion.ConversionClient conversionClient)
        {
            _conversionClient = conversionClient;
        }

        [HttpGet]
        public async Task<ConversionResult> GetConversion(string from, double quantity, string to)
        {
            var conversionRequest = new ConversionRequest
            {
                From = from,
                Quantity = quantity,
                To = to
            };
            var res = _conversionClient.GetConversion(conversionRequest);
            return res;
        }
    }
}