
using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using System;

namespace Repositories
{
    public class StocksRepository : IStocksRepositories
    {
        private readonly StockMarketDbContext _db;
        public StocksRepository(StockMarketDbContext dbContext)
        {
            _db = dbContext;
        }


        public async Task<BuyOrder> CreateBuyOrder(BuyOrder buyOrder)
        {
            _db.BuyOrders.Add(buyOrder);
            await _db.SaveChangesAsync();

            return buyOrder;

        }

        public async Task<SellOrder> CreateSellOrder(SellOrder sellOrder)
        {
            _db.SellOrders.Add(sellOrder);
            await _db.SaveChangesAsync();

            return sellOrder;
        }

        public async Task<List<BuyOrder>> GetBuyOrders()
        {
            return await _db.BuyOrders
                                        .OrderByDescending(temp => temp.DateAndTimeOfOrder)
                                          .ToListAsync();
        }

        public async Task<List<SellOrder>> GetSellOrders()
        {
            return await _db.SellOrders
                                .OrderByDescending(temp => temp.DateAndTimeOfOrder)
                                    .ToListAsync();
        }
    }
}
