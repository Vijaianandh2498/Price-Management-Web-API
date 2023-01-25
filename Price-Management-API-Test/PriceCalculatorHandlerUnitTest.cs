using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Moq;
using Price_Management_API.Controllers;
using Price_Management_API.Handlers;
using Price_Management_Common.Interfaces;
using Price_Management_Common.Requests;
using Price_Management_Common.Responses;

namespace Price_Management_API_Test
{
    public class PriceCalculatorHandlerUnitTest
    {
        private IWriteHandler<GetPriceQuotesCommandRequest, GetPriceQuotesCommandResponse> _getPriceQuotesCommandHandler;

        public PriceCalculatorHandlerUnitTest()
        {
            _getPriceQuotesCommandHandler = new GetPriceQuotesCommandHandler();
        }

        [Fact]
        public async Task PerformPriceCalc_Success()
        {
            GetPriceQuotesCommandRequest getPriceQuotesCommandRequest = new()
            {
                Delivery_postcode = "EC2A3LT",
                Pickup_postcode = "SW1A1AA",
                Vehicle = "small_van"
            };
            
            var result = await _getPriceQuotesCommandHandler.WriteOperationAsync(
                getPriceQuotesCommandRequest,CancellationToken.None).ConfigureAwait(false);

            Assert.NotNull(result);
            Assert.Equal(getPriceQuotesCommandRequest.Vehicle, result.Vehicle);
            Assert.NotNull(result.PriceList);

        }
    }
}