using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceContracts.Helpers;
using ServiceContracts.Interface;
using StockAppWithTagHelpers.Models;

namespace StockAppWithTagHelpers.Controllers
{
    [Route("[controller]")]
    public class StocksController : Controller
    {
        private readonly TradingOptions _tradingOptions;
        private readonly IFinnhubService _finnhubService;

        public StocksController(IOptions<TradingOptions> options , IFinnhubService finnhubService)
        {
                _finnhubService = finnhubService;
                _tradingOptions = options.Value;
        }


        [Route("/")]
        [Route("[action]/{stock?}")]
        [Route("~/[action]/{stock?}")]
        public async Task<IActionResult> Explore(string? stock , bool showAll = false)
        {

            var stocksDictionary =await _finnhubService.GetStocks();

            List<Stock> stocks = new List<Stock>();

            if (stocksDictionary is not null)
            {
                if (!showAll && _tradingOptions.Top25PopularStocks is not null)
                {
                    var Top25PopularStocksList = _tradingOptions.Top25PopularStocks.Split(",");
                    if (Top25PopularStocksList is not null)
                    {
                        stocksDictionary = stocksDictionary
                         .Where(temp => Top25PopularStocksList.Contains(Convert.ToString(temp["symbol"])))
                         .ToList();
                    }
                }

                stocks = stocksDictionary
                    .Select(temp => new Stock() { StockName = Convert.ToString(temp["description"]), StockSymbol = Convert.ToString(temp["symbol"]) })
                           .ToList();
            }

            ViewBag.stock = stock;
            return View(stocks);
        }
    }
}
