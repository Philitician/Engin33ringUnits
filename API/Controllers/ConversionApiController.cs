using System;
using System.Threading.Tasks;
using ConversionGrpcService;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [ApiController]
    [Route("api/conversion")]
    public class ConversionApiController : ControllerBase
    {
        private readonly Conversion.ConversionClient _conversionClient;
        private readonly ILogger<ConversionApiController> _logger;

        public ConversionApiController(Conversion.ConversionClient conversionClient, ILogger<ConversionApiController> logger)
        {
            _conversionClient = conversionClient;
            _logger = logger;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetConversion(string from, double quantity, string to)
        {
            try
            {
                var conversionRequest = new ConversionRequest
                {
                    From = from,
                    Quantity = quantity,
                    To = to
                };
                ConversionResult response = await _conversionClient.GetConversionAsync(conversionRequest);
                return Ok(response);
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e.Message);
                _logger.LogInformation(e.Message);
                return BadRequest(e.Message);
            }
            catch (RpcException e)
            {
                Console.WriteLine(e.Message);
                _logger.LogInformation(e.Message);
                return NotFound(e.Status.Detail);
            }
        }
    }
}