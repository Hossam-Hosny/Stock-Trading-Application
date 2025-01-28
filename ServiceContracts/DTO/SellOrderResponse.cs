

using Entities;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO
{
    public class SellOrderResponse : IOrderResponse
    {
        public Guid SellOrderID { get; set; }
        [Required(ErrorMessage = "Stock Symbol can not be null ")]
        public string StockSymbol { get; set; }
        [Required(ErrorMessage = "Stock Name can not be null ")]
        public string StockName { get; set; }
        public DateTime DateAndTimeOfOrder { get; set; }
        [Range(minimum: 1, maximum: 100_000, ErrorMessage = "You can buy maximum of 100_000 in the single order. Minumum is 1")]
        public uint Quantity { get; set; }
        [Range(minimum: 1, maximum: 10_000, ErrorMessage = "The Maximum Price of Stock is 10_000. Minumum is 1")]
        public double Price { get; set; }
        public double TradeAmount { get; set; }

        public IOrderResponse.OrderType TypeOfOrder => IOrderResponse.OrderType.SellOrder;

        public override bool Equals(object? obj)
        {
            if (obj is null) { return false; }
            if (obj is not SellOrderResponse) { return false; }
            SellOrderResponse sellOrderResponse = (SellOrderResponse)obj;
            return SellOrderID == sellOrderResponse.SellOrderID &&
                StockSymbol == sellOrderResponse.StockSymbol &&
                StockName == sellOrderResponse.StockName &&
                Price == sellOrderResponse.Price &&
                Quantity == sellOrderResponse.Quantity &&
                DateAndTimeOfOrder == sellOrderResponse.DateAndTimeOfOrder &&
                TradeAmount == sellOrderResponse.TradeAmount;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"Sell Order ID: {SellOrderID}, Stock Symbol: {StockSymbol}, Stock Name: {StockName}, Date and Time of Sell Order: {DateAndTimeOfOrder.ToString("dd MMM yyyy hh:mm ss tt")}, Quantity: {Quantity}, Sell Price: {Price}, Trade Amount: {TradeAmount}";
        }

    }
    public static class SellOrderExtensions
    {
        public static SellOrderResponse ToSellOrderResponse(this SellOrder sellOrder)
        {
            return new SellOrderResponse()
            {
                DateAndTimeOfOrder = sellOrder.DateAndTimeOfOrder,
                Price = sellOrder.Price,
                Quantity = sellOrder.Quantity,
                SellOrderID = sellOrder.SellOrderID,
                StockName = sellOrder.StockName,
                StockSymbol = sellOrder.StockSymbol,
                TradeAmount = sellOrder.Price * sellOrder.Quantity
            };
        }
    }
}
