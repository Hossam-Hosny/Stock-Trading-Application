﻿using ServiceContracts.DTO;

namespace StockAppWithTagHelpers.Models
{
    public class Orders
    {

        public List<BuyOrderResponse> BuyOrders { get; set; } = new List<BuyOrderResponse>();
        public List<SellOrderResponse> SellOrders { get; set; } = new List<SellOrderResponse>();
    }
}
