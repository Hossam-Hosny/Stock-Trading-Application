using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using ServiceContracts.CustomValidation;
using ServiceContracts.DTO;
using ServiceContracts.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class StocksService : IStockService
    {
        private readonly IStocksRepositories _stocksRepository;
        public StocksService(IStocksRepositories stocksRepositories)
        {
            _stocksRepository = stocksRepositories;
        }


        public async Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
        {
            if (buyOrderRequest is null) { throw new ArgumentNullException(nameof(buyOrderRequest)); }

            ValidationHelper.ModelValidation(buyOrderRequest);

            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            buyOrder.BuyOrderID = Guid.NewGuid();

            BuyOrder buyOrderFromRepo = await _stocksRepository.CreateBuyOrder(buyOrder);


            return buyOrderFromRepo.ToBuyOrderResponse();



        }

        public async Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest)
        {
            if (sellOrderRequest is null) { throw new ArgumentNullException(nameof(sellOrderRequest)); }

            ValidationHelper.ModelValidation(sellOrderRequest);

            SellOrder sellOrder = sellOrderRequest.ToSellOrder();

            sellOrder.SellOrderID = Guid.NewGuid();

            SellOrder sellOrderFromRepo = await _stocksRepository.CreateSellOrder(sellOrder);

            return sellOrderFromRepo.ToSellOrderResponse();

        }

        public async Task<List<BuyOrderResponse>> GetBuyOrders()
        {
            var buyOrders = await _stocksRepository.GetBuyOrders();
            return buyOrders.Select(temp => temp.ToBuyOrderResponse()).ToList();
        }

        public async Task<List<SellOrderResponse>> GetSellOrders()
        {
            var sellOrders = await _stocksRepository.GetSellOrders();
            return sellOrders.Select(temp => temp.ToSellOrderResponse()).ToList();
        }
    }
}
