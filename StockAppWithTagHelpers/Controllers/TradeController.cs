using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Rotativa.AspNetCore;
using ServiceContracts.DTO;
using ServiceContracts.Helpers;
using ServiceContracts.Interface;
using StockAppWithTagHelpers.Models;

namespace StockAppWithTagHelpers.Controllers
{
    [Route("[controller]")]
    public class TradeController : Controller
    {
        private readonly IFinnhubService _finnhubService;
        private readonly IStockService _stockService;
        private readonly TradingOptions _tradingOptions;
        private readonly IConfiguration _configuration;

        public TradeController(IFinnhubService finnhubService, IStockService stockService, IOptions<TradingOptions> tradingOptions, IConfiguration configuration)
        {
            _finnhubService = finnhubService;
            _stockService = stockService;
            _tradingOptions = tradingOptions.Value;
            _configuration = configuration;
        }



        //[Route("/")]
        [Route("[action]/{stockSymbol}")]
        [Route("~/[controller]/{stockSymbol}")]
        public async Task<IActionResult> Index(string stockSymbol)
        {
            if (!string.IsNullOrEmpty(stockSymbol)) {stockSymbol= "MSFT"; }

            Dictionary<string, object>? CompanyProfileDicationary =await _finnhubService.GetCompanyProfile(stockSymbol);

            Dictionary<string, object>? StockQuoteDictionary =await _finnhubService.GetStockPriceQuote(stockSymbol);


            StockTrade stockTrade = new StockTrade() { StockSymbol = stockSymbol };

            if (CompanyProfileDicationary is not null && StockQuoteDictionary is not null)
            {
                stockTrade = new StockTrade()
                {
                    StockSymbol = CompanyProfileDicationary["ticker"].ToString(),
                    StockName = CompanyProfileDicationary["name"].ToString(),
                    Quantity = _tradingOptions.DefaultOrderQuantity ?? 0,
                    Price = Convert.ToDouble(StockQuoteDictionary["c"].ToString())


                };
            }

            ViewBag.FinnhubToken = _configuration["FinnhubToken"];
            return View(stockTrade);

        }
        [Route ("[action]")]
        [HttpPost]
        public async Task<IActionResult> BuyOrder(BuyOrderRequest buyOrderRequest)
        {
            buyOrderRequest.DateAndTimeOfOrder = DateTime.Now ;

            ModelState.Clear();
            TryValidateModel(buyOrderRequest);

            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors)
                                                    .Select(e => e.ErrorMessage)
                                                    .ToList();

                StockTrade stockTrade = new StockTrade()
                {
                    StockName = buyOrderRequest.StockName,
                    Quantity = buyOrderRequest.Quantity,
                    StockSymbol= buyOrderRequest.StockSymbol
                };

                return View("Index", stockTrade);

            }

            BuyOrderResponse buyOrderResponse = await _stockService.CreateBuyOrder(buyOrderRequest);

            return RedirectToAction(nameof(Orders));

        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> SellOrder(SellOrderRequest sellOrderRequest) 
        {
            sellOrderRequest.DateAndTimeOfOrder = DateTime.Now;

            ModelState.Clear();
            TryValidateModel(sellOrderRequest);

            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                StockTrade stockTrade = new StockTrade() { StockName = sellOrderRequest.StockName, Quantity = sellOrderRequest.Quantity, StockSymbol = sellOrderRequest.StockSymbol };
                return View("Index", stockTrade);
            }

            SellOrderResponse sellOrderResponse =await _stockService.CreateSellOrder(sellOrderRequest);

            return RedirectToAction(nameof(Orders));

        }

        [Route("[action]")]
        public async Task<IActionResult> Orders()
        {

            List<BuyOrderResponse> buyOrderResponses =await _stockService.GetBuyOrders();
            List<SellOrderResponse> sellOrderResponses =await _stockService.GetSellOrders();

            Orders orders = new Orders() 
            { 
            
                BuyOrders= buyOrderResponses,
                SellOrders = sellOrderResponses
            
            };

            ViewBag.TradingOptions = _tradingOptions;

            return View(orders);


        }
        [Route("[action]")]
        public async Task<IActionResult> OrdersPDF()
        {
            List<IOrderResponse> orders = new List<IOrderResponse>();

            orders.AddRange(await _stockService.GetBuyOrders());
            orders.AddRange(await _stockService.GetSellOrders());

            orders = orders.OrderByDescending(temp => temp.DateAndTimeOfOrder).ToList();

            ViewBag.TradingOptions = _tradingOptions;

            return new ViewAsPdf("OrdersPDF" , orders , ViewData)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins() { Top = 20  , Right = 20, Bottom = 20, Left = 20 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };

        }



    }
}
