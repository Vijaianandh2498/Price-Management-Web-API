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
    public class PriceCalculatorControllerUnitTest
    {
        private PriceCalculatorController _priceCalculatorController;
        private Mock<IWriteHandler<GetPriceQuotesCommandRequest, GetPriceQuotesCommandResponse>> _getPriceQuotesCommandHandler;
        private Mock<ILogger<PriceCalculatorController>> _logger;

        public PriceCalculatorControllerUnitTest()
        {
            _getPriceQuotesCommandHandler = new Mock<IWriteHandler<GetPriceQuotesCommandRequest, GetPriceQuotesCommandResponse>>();
            _logger = new Mock<ILogger<PriceCalculatorController>>();
            _priceCalculatorController = new PriceCalculatorController(_getPriceQuotesCommandHandler.Object, _logger.Object);
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
            _getPriceQuotesCommandHandler.Setup(x => x.WriteOperationAsync(It.IsAny<GetPriceQuotesCommandRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetPriceQuotesCommandResponse
            {
                Delivery_postcode = getPriceQuotesCommandRequest.Delivery_postcode,
                Pickup_postcode = getPriceQuotesCommandRequest.Pickup_postcode,
                Vehicle = getPriceQuotesCommandRequest.Vehicle,
                PriceList = new List<PriceList>
                {
                    new PriceList
                    {
                        delivery_time = 1,
                        Price=500,
                        service="RoyalPackages"
                    }
                }
            });
            var result = await _priceCalculatorController.Quotes(getPriceQuotesCommandRequest).ConfigureAwait(false);

            Assert.NotNull(result);
            Assert.Equal(getPriceQuotesCommandRequest.Vehicle, result.Vehicle);
            Assert.NotNull(result.PriceList);

        }
    }
}