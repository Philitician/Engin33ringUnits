using System.Threading.Tasks;
using EngineeringUnitsCore.DLA.AccessorContracts;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace EngineeringUnitsCore.Services
{
    public class ConversionService : Conversion.ConversionBase
    {
        private readonly ILogger<ConversionService> _logger;
        private readonly IUnitOfMeasureAccessor _baseUnitAccessor;
        private readonly ICustomaryUnitAccessor _customaryUnitAccessor;

        public ConversionService(ILogger<ConversionService> logger, IUnitOfMeasureAccessor baseUnitAccessor, ICustomaryUnitAccessor customaryUnitAccessor)
        {
            _logger = logger;
            _baseUnitAccessor = baseUnitAccessor;
            _customaryUnitAccessor = customaryUnitAccessor;
        }

        public override async Task<ConversionResult> GetConversion(ConversionRequest request, ServerCallContext context)
        {
            var baseQuantity = await GetBaseConversion(request.From, request.Quantity);
            var conversionResult = await GetConversionResult(request.To, baseQuantity);

            // conversion to base unit requires a accessing the DB
            // a third time to get base unit name
            if (conversionResult.Name == null)
            {
                var baseUnit = await _baseUnitAccessor.Get(conversionResult.Unit);
                conversionResult.Name = baseUnit.Annotation;
            }

            return conversionResult;
            
        }
        
        private async Task<double> GetBaseConversion(string from, double quantity)
        {
            var fromUnit = await _customaryUnitAccessor.Get(from);
            
            if (fromUnit == null)
                return quantity;

            var conversion = fromUnit.ConversionToBaseUnit;
            return ConvertQuantity(quantity, conversion.A, conversion.B, conversion.C);
        }

        private async Task<ConversionResult> GetConversionResult(string to, double quantity)
        {
            var toUnit = await _customaryUnitAccessor.Get(to);

            if (toUnit == null)
                return new ConversionResult
                {
                    Unit = to,
                    Quantity = quantity
                };

            var conversion = toUnit.ConversionToBaseUnit;

            return new ConversionResult
            {
                Unit = toUnit.Id,
                Name = toUnit.Name,
                // C and B positions are swapped
                Quantity = ConvertQuantity(quantity, conversion.A, conversion.C, conversion.B)
            };
        }
        
        private static double ConvertQuantity(double x, double A, double B, double C)
        {
            var offset = A / C;
            var scale = B / C;
            return offset + scale * x;
        }
    }
}