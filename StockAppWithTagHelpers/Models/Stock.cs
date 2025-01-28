namespace StockAppWithTagHelpers.Models
{
    public class Stock
    {

        public string? StockSymbol { get; set; }    
        public string? StockName { get; set; }



        public override bool Equals(object? obj)
        {
            if (obj  == null) return false;
            if (obj is not Stock) return false;

            Stock model = (Stock)obj;
            return model.StockSymbol == StockSymbol &&
                        model.StockName == StockName;



        }

    }
}
