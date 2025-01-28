using Microsoft.Extensions.Configuration;
using RepositoryContracts;
using ServiceContracts.Interface;
using System.Linq.Expressions;
using System.Text.Json;


namespace Services
{
    public class FinnhubService : IFinnhubService
    {
        private readonly IFinnhubRepository _finnhubRepository;

        public FinnhubService(IFinnhubRepository finnhub)
        {
           _finnhubRepository = finnhub;
        }

        public  async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
        {

            var responseDictionary = await _finnhubRepository.GetCompanyProfile(stockSymbol);
            return responseDictionary;
            
            
        }

        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
        {

            var responseDictionary = await _finnhubRepository.GetStockPriceQuote(stockSymbol);
            return responseDictionary;
        }

        public async Task<List<Dictionary<string, string>>?> GetStocks()
        {
            var responseDictionary = await _finnhubRepository.GetStocks();
            return responseDictionary;
        }

        public async Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch)
        {
            var responseDictionary = await _finnhubRepository.SearchStocks(stockSymbolToSearch);
            return responseDictionary;
        }
    }
}
