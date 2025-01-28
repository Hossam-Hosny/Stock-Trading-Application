

using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class SellOrder
    {
        public Guid SellOrderID { get; set; }
        [Required(ErrorMessage ="Stock Symbol can not be null ")]
        public string StockSymbol { get; set; }
        [Required(ErrorMessage = "Stock Name can not be null ")]
        public string StockName { get; set; }
        public DateTime DateAndTimeOfOrder { get; set; }
        [Range(minimum: 1, maximum: 100_000, ErrorMessage = "You can buy maximum of 100_000 in the single order. Minumum is 1")]
        public uint Quantity { get; set; }
        [Range(minimum: 1, maximum: 10_000, ErrorMessage = "The Maximum Price of Stock is 10_000. Minumum is 1")]
        public double Price { get; set; }
    }
}
