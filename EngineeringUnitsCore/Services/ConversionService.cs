using System;
using System.Threading.Tasks;
using ConversionGrpcService;
using EngineeringUnitsCore.DAL.AccessorContracts;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace EngineeringUnitsCore.Services
{
    public class ConversionService : Conversion.ConversionBase
    {
        private readonly ILogger<ConversionService> _logger;
        private readonly IUnitOfMeasureAccessor _baseUnitAccessor;
        private readonly ICustomaryUnitAccessor _customaryUnitAccessor;

        public ConversionService(ILogger<ConversionService> logger, IUnitOfMeasureAccessor baseUnitAccessor,
            ICustomaryUnitAccessor customaryUnitAccessor)
        {
            _logger = logger;
            _baseUnitAccessor = baseUnitAccessor;
            _customaryUnitAccessor = customaryUnitAccessor;
        }

        
        public override async Task<ConversionResult> GetConversion(ConversionRequest request, ServerCallContext context)
        {
            ConversionResult baseConversion;
            try
            {
                baseConversion = await GetBaseConversion(request.From, request.Quantity);
            }
            catch (RpcException e)
            {
                Console.WriteLine(e);
                _logger.LogError(e.Message);
                throw new RpcException(e.Status);
            }

            ConversionResult conversionResult;
            
            try
            {
                conversionResult = await GetConversionResult(baseConversion, request.To);
            }
            catch (RpcException e)
            {
                Console.WriteLine(e);
                _logger.LogError(e.Message);
                throw new RpcException(e.Status);
            }
            
            return conversionResult;
        }

        private async Task<ConversionResult> GetBaseConversion(string from, double quantity)
        {
            var fromUnit = await _customaryUnitAccessor.Get(from);

            if (fromUnit == null)
            {
                var baseUnit = await _baseUnitAccessor.Get(from);

                if (baseUnit == null)
                    throw new RpcException(new Status(StatusCode.Cancelled,"Invalid id or alias of source unit"));
                return new ConversionResult {Unit = from, Quantity = quantity};
            }

            var conversion = fromUnit.ConversionToBaseUnit;
            var baseQuantity = ConvertQuantity(quantity, conversion.A, conversion.B, conversion.C);

            return new ConversionResult
            {
                Unit = conversion.BaseUnit, Quantity = baseQuantity
            };
        }

        private async Task<ConversionResult> GetConversionResult(ConversionResult baseConversion, string to)
        {
            var toUnit = await _customaryUnitAccessor.Get(to);

            ConversionResult conversionResult;

            if (toUnit == null)
            {
                var baseUnit = await _baseUnitAccessor.Get(to);

                if (baseUnit == null)
                    throw new RpcException(new Status(StatusCode.Cancelled,
                        "Invalid id or alias of destination unit"));
                if (baseUnit.Id != baseConversion.Unit)
                    throw new RpcException(new Status(StatusCode.Cancelled, 
                        "The units do not share a common base"));
                
                conversionResult = new ConversionResult
                {
                    Unit = baseUnit.Id,
                    Name = baseUnit.Name,
                    Quantity = Math.Round(baseConversion.Quantity, 6, MidpointRounding.AwayFromZero)
                };
            }
            else
            {
                var conversion = toUnit.ConversionToBaseUnit;

                if (conversion.BaseUnit != baseConversion.Unit)
                    throw new RpcException(new Status(StatusCode.Cancelled, 
                        "The units do not share a common base"));
            
                conversionResult = new ConversionResult
                {
                    Unit = toUnit.Id,
                    Name = toUnit.Name,
                    // C and B positions are swapped
                    Quantity = Math.Round(ConvertQuantity(baseConversion.Quantity, conversion.A, conversion.C, conversion.B), 6, MidpointRounding.AwayFromZero)
                };
            }
            
            return conversionResult;
        }

        private static double ConvertQuantity(double x, double A, double B, double C)
        {
            var offset = A / C;
            var scale = B / C;
            return offset + scale * x;
        }
    }
}