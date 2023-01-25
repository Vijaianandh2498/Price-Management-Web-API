using Microsoft.AspNetCore.Mvc;
using Price_Management_API.Handlers;
using Price_Management_Common.Interfaces;
using Price_Management_Common.Requests;
using Price_Management_Common.Responses;

namespace Price_Management_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PriceCalculatorController : ControllerBase
    {
        private readonly IWriteHandler<GetPriceQuotesCommandRequest, GetPriceQuotesCommandResponse> _getPriceQuotesCommandHandler;
        private readonly ILogger<PriceCalculatorController> _logger;
        
        public PriceCalculatorController(
            IWriteHandler<GetPriceQuotesCommandRequest, GetPriceQuotesCommandResponse> getPriceQuotesCommandHandler,
            ILogger<PriceCalculatorController> logger)
        {
            _getPriceQuotesCommandHandler = getPriceQuotesCommandHandler;
            _logger = logger;
        }

        [HttpPost("quotes")]
        public async Task<GetPriceQuotesCommandResponse> Quotes(
            [FromBody]GetPriceQuotesCommandRequest getPriceQuotesCommandRequest)
        {
            var response = await _getPriceQuotesCommandHandler.WriteOperationAsync(
                getPriceQuotesCommandRequest, CancellationToken.None).ConfigureAwait(false);
            return await Task.FromResult(response).ConfigureAwait(false);
        }
    }
}