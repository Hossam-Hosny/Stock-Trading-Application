

using Entities;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks.Dataflow;

namespace ServiceContracts.DTO
{
    public class BuyOrderResponse : IOrderResponse
    {
        public Guid BuyOrderID { get; set; }
        [Required(ErrorMessage = "Stock Symbol can not be null ")]
        public string StockSymbol { get; set; }
        [Required(ErrorMessage = "Stock Name can not be null")]
        public string StockName { get; set; }
        public DateTime DateAndTimeOfOrder { get; set; }
        [Range(minimum: 1, maximum: 100_000, ErrorMessage = "You can buy maximum of 100_000 in the single order. Minumum is 1")]
        public uint Quantity { get; set; }
        [Range(minimum: 1, maximum: 10_000, ErrorMessage = "The Maximum Price of Stock is 10_000. Minumum is 1")]
        public double Price { get; set; }
        public double TradeAmount { get; set; }

        public IOrderResponse.OrderType TypeOfOrder => IOrderResponse.OrderType.BuyOrder;

        public override bool Equals(object? obj)
        {
            if (obj is null) { return false; }
            if (obj is not BuyOrderRequest) { return false; }
            BuyOrderResponse buyOrderResponse = (BuyOrderResponse)obj;
            return BuyOrderID == buyOrderResponse.BuyOrderID &&
                StockName == buyOrderResponse.StockName &&
                StockSymbol == buyOrderResponse.StockSymbol &&
                Price == buyOrderResponse.Price &&

                DateAndTimeOfOrder == buyOrderResponse.DateAndTimeOfOrder &&
                Quantity == buyOrderResponse.Quantity;
                   
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override string ToString()
        {
            return $"Buy Order ID: {BuyOrderID}, Stock Symbol: {StockSymbol}, Stock Name: {StockName}, Date and Time of Buy Order: {DateAndTimeOfOrder.ToString("dd MMM yyyy hh:mm ss tt")}, Quantity: {Quantity}, Buy Price: {Price}, Trade Amount: {TradeAmount}";
        }

    }
    public static class BuyOrderExtentions
    {
        public static BuyOrderResponse ToBuyOrderResponse(this BuyOrder buyOrder)
        {
            return new BuyOrderResponse()
            {
                BuyOrderID = buyOrder.BuyOrderID,
                DateAndTimeOfOrder=buyOrder.DateAndTimeOfOrder,
                Price=buyOrder.Price,
                Quantity=buyOrder.Quantity,
                StockName = buyOrder.StockName,
                StockSymbol = buyOrder.StockSymbol,
                TradeAmount=buyOrder.Price * buyOrder.Quantity
            };
        }
    }
}
