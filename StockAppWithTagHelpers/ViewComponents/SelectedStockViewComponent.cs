using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceContracts.Helpers;
using ServiceContracts.Interface;

namespace StockAppWithTagHelpers.ViewComponents
{
    public class SelectedStockViewComponent:ViewComponent
    {
        private readonly TradingOptions _tradingOptions;
        private readonly IStockService _stocksService;
        private readonly IFinnhubService _finnhubService;
        private readonly IConfiguration _configuration;

        public SelectedStockViewComponent(IOptions<TradingOptions> tradingOptions, IStockService stocksService, IFinnhubService finnhubService, IConfiguration configuration)
        {
            _tradingOptions = tradingOptions.Value;
            _stocksService = stocksService;
            _finnhubService = finnhubService;
            _configuration = configuration;
        }



        public async Task<IViewComponentResult> InvokeAsync(string? stockSymbol)
        {
            Dictionary<string, object>? companyProfileDict = null;

            if (stockSymbol != null)
            {
                companyProfileDict = await _finnhubService.GetCompanyProfile(stockSymbol);
                var stockPriceDict = await _finnhubService.GetStockPriceQuote(stockSymbol);
                if (stockPriceDict != null && companyProfileDict != null)
                {
                    companyProfileDict.Add("price", stockPriceDict["c"]);
                }
            }

            if (companyProfileDict != null && companyProfileDict.ContainsKey("logo"))
                return View(companyProfileDict);
            else
                return Content("");
        }
    }
}





