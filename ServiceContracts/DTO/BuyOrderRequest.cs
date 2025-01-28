
using Entities;
using ServiceContracts.CustomValidation;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO
{
    public class BuyOrderRequest
    {
        [Required(ErrorMessage = "Stock Symbol can not be null ")]
        public string StockSymbol { get; set; }
        [Required(ErrorMessage = "Stock Name can not be null")]
        public string StockName { get; set; }
        [DateTimeValidatiion]
        public DateTime DateAndTimeOfOrder { get; set; }
        [Range(minimum: 1, maximum: 100_000, ErrorMessage ="You can buy maximum of 100_000 in")]
        public uint Quantity { get; set; }
        [Range(minimum: 1, maximum: 10_000, ErrorMessage = "The Maximum Price of Stock is 10_000. Minumum is 1")]
        public double Price { get; set; }




        public BuyOrder ToBuyOrder()
        {
            return new BuyOrder()
            {
                StockSymbol = StockSymbol,
                StockName = StockName,
                DateAndTimeOfOrder = DateAndTimeOfOrder,
                Price = Price,
                Quantity = Quantity
            };
        }
    }
}
