

using Entities;

namespace RepositoryContracts
{
    public interface IStocksRepositories
    {

        /// <summary>
        /// Creates a buy order
        /// </summary>
        /// <param name="buyOrder">Buy order object</param>
        Task<BuyOrder> CreateBuyOrder (BuyOrder buyOrder);

        /// <summary>
        /// Creates a buy order
        /// </summary>
        /// <param name="sellOrder">Sell order object</param>
        Task<SellOrder> CreateSellOrder(SellOrder sellOrder);

        /// <summary>
        /// Returns all existing buy orders
        /// </summary>
        /// <returns>Returns a list of objects of BuyOrder type</returns>
        Task<List<BuyOrder>> GetBuyOrders();

        /// <summary>
        /// Returns all existing sell orders
        /// </summary>
        /// <returns>Returns a list of objects of SellOrder type</returns>
        Task<List<SellOrder>> GetSellOrders();


    }
}
